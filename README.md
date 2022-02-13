# Property Manager

Property Manager is an addon for [MMORPG KIT](https://assetstore.unity.com/packages/templates/systems/mmorpg-kit-2d-3d-survival-110188) that enables players to purchase buildings setup in the editor. It is an incomplete system shared primarily for _learning purposes_. Feel free to adapt for your own needs.

By default, MMORPG Kit enables players to [construct their own structures at runtime](https://suriyun-production.github.io/mmorpg-kit-docs/#/pages/008-building-building-item-building-entity-building-material-building-area?id=building-items), for example gameplay from Minecraft or Fortnite. In this implementation, buildings are instead manually placed in the editor and then the creator is set to the purchasing player in-game.

### Installation

1. [:raised_back_of_hand: core mod] add partial to DoorEntity class found in `Core/Scripts/GamePlay/BuildingSystems/DoorEntity.cs`

```C#
public partial class DoorEntity : BuildingEntity
```

2. [:raised_back_of_hand: core mod] add new string to public enum UITextKeys found in `Core/Scripts/Language/Language.cs`

```C#
        // Addon - Property Manager
        UI_PROPERTY_MANAGER_PURCHASE_SUCCESS,
```

and to public static class DefaultLocale

```C#
            // Addon - Property Manager
            Texts.Add(UITextKeys.UI_PROPERTY_MANAGER_PURCHASE_SUCCESS.ToString(), "Property purchased");
```

3. modify your CanvasGameplay prefab and replace UICurrentBuilding prefab with UICurrentBuilding_PropertyManager found in `DenariiGames/UnityMultiplayerARPG/PropertyManager/Prefabs/UI/Share`

### Usage

Here is where it gets tricky since our implementation will differ substantially from your own. Our building generation and save editor tooling is not part of this mod. The manual steps are as follows (which is why we call it an incomplete system without the automatic building generation):

1. We first created a new building entity named DoorPlacementEntity copied from the DoorFrameEntity. It is identical except its Building type is set to Foundation instead of Wall. This enables the DoorPlacementEntity to be put anywhere.

2. Create an item associated with DoorPlacementEntity and add to your game database and optionally the Shop inventory (for purchase by your character).

3. Modify or create a copy of the DoorEntity. Select Is Property Entrance checkbox on the Door Entity component.

4. Place the DoorPlacementEntity in-game to write to database. Place a door in the DoorPlacementEntity to write to database.

5. Modify the buildings table to remove the creatorId and creatorName from the saved DoorPlacementEntity and DoorEntity.

6. Add the price (integer) for the building in the DoorEntity extraData column.

### Plan of intent

Property Manager is a core system being built for our own game. We have a wide ranging list of features we plan to add in no particular order. 

- [x] purchase building
- [x] set building price
- [ ] rent building
- [ ] sell building
- [ ] see list of buildings
- [ ] set building type: residence, warehouse, shop
- [ ] place building items appropriate to building type
- [ ] hire NPCs appropriate to building type
- [ ] set shop items and pricing
- [ ] collect taxes on shop
