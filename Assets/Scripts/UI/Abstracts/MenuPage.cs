using UnityEngine;

namespace UI.Abstracts
{
    public enum MenuPageType
    {
        Base, Inventory
    }
    
    public abstract class MenuPage : MonoBehaviour
    {
        public MenuPageType Type => _type;
        
        [SerializeField] private MenuPageType _type = MenuPageType.Base;
        
        public abstract void Show();

        public abstract void Hide();
    }
}