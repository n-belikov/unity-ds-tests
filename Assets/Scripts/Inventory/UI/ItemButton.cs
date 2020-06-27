using Inventory.Item.Data.Abstracts;
using UI.Elements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class ItemButton : MonoBehaviour
    {
        [System.Serializable]
        public class ButtonEvent : UnityEvent<ItemButton>
        {
        }

        public ButtonEvent onClick;
        
        [SerializeField] private Image _image;
        [SerializeField] private FillImageScript _equipIcon;
        
        public int Index { get; private set; } = 0;

        public void SetItem(InventoryItem item, int index)
        {
            _image.sprite = item.Item.Icon;
            Index = index;

            var canEquip = item.Item as IWeaponItemData != null;
            
            _equipIcon.gameObject.SetActive(canEquip);
            _equipIcon.SetState(item.equip);
        }

        public void OnClick()
        {
            onClick?.Invoke(this);
        }

        public void SetItem(InventoryItem item)
        {
            SetItem(item, Index);
        }
    }
}