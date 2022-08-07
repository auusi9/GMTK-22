using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    public class DieUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _selectedUI;
        [SerializeField] private Shadow _shadow;

        private Die _die;

        public Action<DieUI> DieSelected;

        public Die Die => _die;

        public void SetDie(Die die)
        {
            _die = die;

            _icon.sprite = _die.GetFirstSprite(out Color shadowColor);
            _shadow.effectColor = shadowColor;
            SetDeselected();
        }

        public void SetFace(Face face)
        {
            if (face == null)
            {
                _icon.sprite = _die.GetDefaultSprite(out Color shadowColor);
                _shadow.effectColor = shadowColor;
                return;
            }
            
            _icon.sprite = face.Sprite;
            _shadow.effectColor = face.ShadowColor;
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
            _icon.sprite = _die.GetRandomSprite(_icon.sprite, out Color shadowColor);
            _shadow.effectColor = shadowColor;
        }
    }
}