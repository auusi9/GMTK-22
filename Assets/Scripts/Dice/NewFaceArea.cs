using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    public class NewFaceArea : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Shadow _shadow;
        [SerializeField] private string _textFormat = "{0} x{1}";
        
        private Face _face;
        
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
            _description.text = string.Format(_textFormat, _face.Resource.ResourceName, _face.CurrentReward);
        }
    }
}