/**
 * BasePlayerCharacterController_PropertyManager
 * Author: Denarii Games
 * Version: 1.0
 *
 * Simple dialog implementation for Property Manager. Future implementations wil likely
 * have a custom UI to handle additional functionality.
 */

using UnityEngine;

namespace MultiplayerARPG
{
    public abstract partial class BasePlayerCharacterController
    {
        public virtual void ShowPropertyManager()
        {
            if (TargetBuildingEntity == null) return;

            uint objectId = TargetBuildingEntity.ObjectId;

            //is building already owned?
            if (TargetBuildingEntity.CreatorId.Length > 0)
            {
                string ownerName = TargetBuildingEntity.CreatorId == PlayerCharacterEntity.Id ? "you" : TargetBuildingEntity.CreatorName;
                UISceneGlobal.Singleton.ShowMessageDialog(
                    "Property Manager",
                    $"This building is owned by {ownerName}."
                );
            }
            else
            {
                //get property price
                int propertyPrice = (TargetBuildingEntity as DoorEntity).PropertyPrice;

                //cannot afford purchase
                if (propertyPrice > PlayerCharacterEntity.Gold)
                {
                    UISceneGlobal.Singleton.ShowMessageDialog(
                        "Property Manager",
                        $"This building is purchasable for {propertyPrice} gold. You do not have enough gold."
                    );
                }
                //can afford purchase
                else
                {
                    UISceneGlobal.Singleton.ShowMessageDialog(
                        "Property Manager",
                        $"This building is purchasable for {propertyPrice} gold. Would you like to purchase the building?",
                        false,
                        true,
                        true,
                        false,
                        null,
                        () => { OnPurchaseBuildingConfirmed(objectId); }
                    );
                }
            }

            DeselectBuilding();
        }

        private void OnPurchaseBuildingConfirmed(uint objectId)
        {
            if (objectId == null) return;
            PlayerCharacterEntity.Building.CallServerPurchaseBuilding(objectId);
        }
    }
}