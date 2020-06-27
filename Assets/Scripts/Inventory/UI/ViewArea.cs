using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class ViewArea : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        
        public void SetItem(InventoryItem item)
        {
            if (_text) {
                _text.text = item.Item.Title;
            }

            if (_textMeshPro) {
                _textMeshPro.text = item.Item.Title;
            }
        }
    }
}