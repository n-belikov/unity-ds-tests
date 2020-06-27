using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Elements
{
    [RequireComponent(typeof(Image))]
    public class FillImageScript : MonoBehaviour
    {
        [SerializeField]
        private Sprite _enabledImage, _disabledImage;
        
        [SerializeField] private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        public void SetState(bool enable)
        {
            _image.sprite = enable ? _enabledImage : _disabledImage;
        }
    }
}
