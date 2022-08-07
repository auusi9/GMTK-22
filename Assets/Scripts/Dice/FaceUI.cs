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
        [SerializeField] private TextMeshProUGUI _sceneryValue;
        [SerializeField] private string _textFormat = "x{0}";
        
        private Face _face;
        
        public event Action<FaceUI> FaceSelected;

        public void SetFace(Face face)
        {
            _face = face;

            if (_face == null)
            {
                _icon.sprite = null;
                _description.text = "Dice face is empty";
                _sceneryValue.text = string.Empty;
                return;
            }

            _icon.sprite = _face.Sprite;
            _shadow.effectColor = _face.ShadowColor;
            _sceneryValue.text = string.Format(_textFormat, _face.CurrentReward);
            _description.text = _face.Description;
        }

        public void Clicked()
        {
            FaceSelected?.Invoke(this);
        }
    }
}