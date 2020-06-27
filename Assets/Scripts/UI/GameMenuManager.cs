using System.Collections.Generic;
using CryoDI;
using Inputs;
using Managers;
using UI.Abstracts;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace UI
{
    public class GameMenuManager : CryoBehaviour
    {
        public UnityEvent<bool> OnOpenEvent => onOpenEvent;
        public bool IsOpen { get; private set; } = false;

        [Dependency] private IInputHandler InputHandler { get; set; }
        
        [SerializeField] private GameObject menuObject;
        // [SerializeField] private InputHandler inputHandler;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private BoolEvent onOpenEvent;
        [SerializeField] private GameObject mainCamera, menuCamera;
        [SerializeField] private List<MenuPage> pages = new List<MenuPage>();

        private MenuPageType _page = MenuPageType.Base;

        private bool _isClicked = false, _canClose = true;

        private void Start()
        {
            if (!playerManager) {
                playerManager = GameObject.FindObjectOfType<PlayerManager>();
            }

            SetVisibleMenu(false);
        }

        private void Update()
        {
            if (GetClickAllMenu() && !_isClicked && (!IsOpen || _canClose)) {
                SetVisibleMenu(!IsOpen);
                _isClicked = true;
            }
            else if (!GetClickAllMenu() && _isClicked) {
                _isClicked = false;
            }


            if (IsOpen) {
                if (!GetClickAll() && !_canClose) {
                    _canClose = true;
                }
                
                if (InputHandler.Menu && _page != MenuPageType.Base) {
                    _canClose = false;
                    ShowByType(MenuPageType.Base);
                }

                if (InputHandler.Inventory && _page != MenuPageType.Inventory) {
                    _canClose = false;
                    ShowByType(MenuPageType.Inventory);
                }
            }
        }

        private bool GetClickAll()
        {
            return InputHandler.Menu || InputHandler.Inventory;
        }

        private bool GetClickAllMenu()
        {
            return (!IsOpen && GetClickAll()) ||
                   (IsOpen && InputHandler.Menu && _page == MenuPageType.Base) || 
                   (IsOpen && InputHandler.Inventory && _page == MenuPageType.Inventory);
        }

        private void ShowByType(MenuPageType type)
        {
            foreach (var page in pages) {
                if (page.Type == type) {
                    page.Show();
                }
                else {
                    page.Hide();
                }
            }

            _page = type;
        }

        public void SetVisibleMenu(bool isOpen)
        {
            menuCamera.SetActive(isOpen);
            IsOpen = isOpen;
            if (IsOpen) {
                ShowByType(MenuPageType.Base);
            }

            menuObject.SetActive(IsOpen);
            playerManager.SetInMenu(IsOpen);
            onOpenEvent?.Invoke(isOpen);
        }
    }
}