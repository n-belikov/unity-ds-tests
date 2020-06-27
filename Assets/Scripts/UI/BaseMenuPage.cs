using UI.Abstracts;
using UnityEngine;

namespace UI
{
    public class BaseMenuPage : MenuPage
    {
        [SerializeField] private GameObject page;

        public override void Show()
        {
            page.SetActive(true);
        }

        public override void Hide()
        {
            page.SetActive(false);
        }
    }
}