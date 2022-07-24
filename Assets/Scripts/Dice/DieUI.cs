using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    public class DieUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _selectedUI;

        private Die _die;

        public Action<DieUI> DieSelected;

        public Die Die => _die;

        public void SetDie(Die die)
        {
            _die = die;

            _icon.sprite = _die.GetFirstSprite();
            SetDeselected();
        }

        public void SetFace(Face face)
        {
            if (face == null)
            {
                _icon.sprite = _die.GetDefaultSprite();
                return;
            }
            
            _icon.sprite = face.Sprite;
        }

        public void SetSelected()
        {
            _selectedUI.gameObject.SetActive(true);
        }

        public void SetDeselected()
        {
            _selectedUI.gameObject.SetActive(false);
        }

        public void Clicked()
        {
            DieSelected?.Invoke(this);
        }

        public void ChangeFace()
        {
            _icon.sprite = _die.GetRandomSprite(_icon.sprite);
        }
    }
}