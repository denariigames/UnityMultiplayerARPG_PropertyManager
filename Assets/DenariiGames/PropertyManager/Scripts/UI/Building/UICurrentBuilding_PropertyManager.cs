/**
 * UICurrentBuilding_PropertyManager
 * Author: Denarii Games
 * Version: 1.0
 */

using UnityEngine;
using UnityEngine.UI;

namespace MultiplayerARPG
{
    public partial class UICurrentBuilding
    {
        public Button buttonPropertyManager;

        [DevExtMethods("Show")]
        protected void UICurrentBuilding_PropertyManager_Show()
        {
            if (buttonPropertyManager == null) return;
            if (Controller.TargetBuildingEntity != null && Controller.TargetBuildingEntity is DoorEntity)
            {
                buttonPropertyManager.gameObject.SetActive((Controller.TargetBuildingEntity as DoorEntity).isPropertyEntrance);
            }
            else buttonPropertyManager.gameObject.SetActive(false);
        }

        public void OnClickShowPropertyManager()
        {
            Controller.ShowPropertyManager();
            Hide();
        }
    }
}