using CryoDI;
using Inventory.Repository;
using Managers.Containers.Abstracts;
using UnityEngine;

namespace Inventory.UI
{
    public class UiInventory : MonoBehaviour
    {
        public GameObject page;
        public Transform content;
        public ItemButton itemPrefab;
        public ViewArea area;

        private InventoryManager _inventoryManager;
        private IInventoryRepository _repository;
        private ItemButton[] _itemButtons;
        private int _currentSelectIndex = -1;
        [Dependency] private ISoundContainer SoundContainer { get; set; }

        public void Init(InventoryManager inventoryManager)
        {
            _inventoryManager = inventoryManager;
            _repository = _inventoryManager.Repository;
        }

        public void ShowGameObject(bool visible)
        {
            page.SetActive(visible);
        }

        public void Show()
        {
            _currentSelectIndex = -1;
            area.gameObject.SetActive(false);

            _itemButtons = new ItemButton[_repository.Count];

            for (int i = 0; i < content.childCount; i++) {
                Destroy(content.GetChild(i).gameObject);
            }

            for (int i = 0; i < _repository.Count; i++) {
                InventoryItem item = _repository.Get(i);
                ItemButton button = _itemButtons[i] = Instantiate(itemPrefab, content) as ItemButton;
                button.gameObject.name = $"button_{i}";
                button.SetItem(item, i);
                button.onClick.AddListener(OnButtonClick);
            }
        }

        private void RefreshItemButtons()
        {
            for (int i = 0; i < _repository.Count; i++) {
                _itemButtons[i].SetItem(
                    _repository.Get(i),
                    i
                );
            }
        }

        public void OnUseClick()
        {
            if (_currentSelectIndex >= 0) {
                _inventoryManager.Equip(_currentSelectIndex);
                RefreshItemButtons();
            }
        }

        public void OnButtonClick(ItemButton button)
        {
            _currentSelectIndex = button.Index;
            area.gameObject.SetActive(true);
            SoundContainer.PlayByType(SoundType.InventoryItemClick);
            area.SetItem(
                _repository.Get(button.Index)
            );
        }
    }
}