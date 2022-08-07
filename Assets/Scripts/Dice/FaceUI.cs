using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    public class FaceUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Shadow _shadow;

        private Face _face;
        
        public Action<FaceUI> FaceSelected;

        public void SetFace(Face face)
        {
            _face = face;

            if (_face == null)
            {
                _icon.sprite = null;
                _description.text = "Dice face is empty";
                return;
            }

            _icon.sprite = _face.Sprite;
            _shadow.effectColor = _face.ShadowColor;
            _description.text = _face.Description;
        }

        public void Clicked()
        {
            FaceSelected?.Invoke(this);
        }
    }
}