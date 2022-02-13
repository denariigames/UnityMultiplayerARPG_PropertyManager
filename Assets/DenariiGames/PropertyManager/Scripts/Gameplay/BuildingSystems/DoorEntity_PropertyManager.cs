/**
 * DoorEntity_PropertyManager
 * Author: Denarii Games
 * Version: 1.0
 *
 * Requires adding partial to the core DoorEntity class. Future implementations may use a 
 * building entity other than door to access Property Manager.
 */

using System.Text;
using UnityEngine;
using LiteNetLibManager;

namespace MultiplayerARPG
{
    public partial class DoorEntity
    {
        [Category(6, "Property Manager")]
        [SerializeField]
        public bool isPropertyEntrance = false;

        [SerializeField]
        protected SyncFieldInt propertyPrice = new SyncFieldInt();

        public int PropertyPrice
        {
            get { return propertyPrice.Value; }
            set { propertyPrice.Value = value; }
        }

        public override string ExtraData
        {
            get
            {
                return new StringBuilder().Append(PropertyPrice).ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;

                string[] splitedTexts = value.Split(':');
                int propertyPrice;
                if (int.TryParse(splitedTexts[0], out propertyPrice))
                PropertyPrice = propertyPrice;
            }
        }
    }
}