/**
 * PlayerCharacterBuildingComponent_PropertyManager
 * Author: Denarii Games
 * Version: 1.0
 *
 * Sets the creatorId and creatorName recursively from the door. Future implementations
 * will need to account for sibling building entities.
 */

using System;
using System.Collections.Generic;
using LiteNetLibManager;
using UnityEngine;

namespace MultiplayerARPG
{
    public partial class PlayerCharacterBuildingComponent
    {
        public bool CallServerPurchaseBuilding(uint objectId)
        {
            if (!CurrentGameplayRule.CanInteractEntity(Entity, objectId))
            {
                ClientGenericActions.ClientReceiveGameMessage(UITextKeys.UI_ERROR_CHARACTER_IS_TOO_FAR);
                return false;
            }

            RPC(ServerPurchaseBuilding, objectId);
            return true;
        }

        [ServerRpc]
        protected void ServerPurchaseBuilding(uint objectId)
        {
#if !CLIENT_BUILD
            if (!Entity.CanDoActions()) return;

            BuildingEntity buildingEntity;
            if (!Manager.TryGetEntityByObjectId(objectId, out buildingEntity)) return;

            //get property price
            int propertyPrice = (buildingEntity as DoorEntity).PropertyPrice;

            //check distance
            if (!Entity.IsGameEntityInDistance(buildingEntity, CurrentGameInstance.conversationDistance))
            {
                GameInstance.ServerGameMessageHandlers.SendGameMessage(ConnectionId, UITextKeys.UI_ERROR_CHARACTER_IS_TOO_FAR);
                return;
            }

            //build recursive list of building hierarchy
            //@todo does not account for any other children of parents
            List<BuildingEntity> buildingHierarchy = new List<BuildingEntity>();
            buildingHierarchy.Add(buildingEntity);

            string ParentId = buildingEntity.ParentId;
            while (ParentId.Length > 0)
            {
                if (GameInstance.ServerBuildingHandlers.TryGetBuilding(ParentId, out buildingEntity))
                {
                    buildingHierarchy.Add(buildingEntity);
                    ParentId = buildingEntity.ParentId;
                }
                else ParentId = "";
            }

            //check if there already is an owner in hierarchy
            foreach (BuildingEntity building in buildingHierarchy)
            {
                if (building.CreatorId.Length > 0)
                {
                    GameInstance.ServerGameMessageHandlers.SendGameMessage(ConnectionId, UITextKeys.UI_ERROR_INVALID_ITEM_DATA);
                    return;
                }
            }

            if (propertyPrice > Entity.Gold)
            {
                GameInstance.ServerGameMessageHandlers.SendGameMessage(ConnectionId, UITextKeys.UI_ERROR_NOT_ENOUGH_GOLD);
                return;
            }

            //remove gold and set creator
            Entity.Gold -= propertyPrice;
            foreach (BuildingEntity building in buildingHierarchy)
            {
                building.CreatorId = Entity.Id;
                building.CreatorName = Entity.CharacterName;
            }

            GameInstance.ServerGameMessageHandlers.SendGameMessage(ConnectionId, UITextKeys.UI_PROPERTY_MANAGER_PURCHASE_SUCCESS);
#endif
        }
    }
}