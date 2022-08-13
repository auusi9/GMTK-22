using System;
using Dice.FaceUIBehaviours;
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
        [SerializeField] private FaceUIBehaviour _faceUIEmpty;
        [SerializeField] private FaceUIBehaviour _faceUIHover;
        [SerializeField] private FaceUIBehaviour _faceUISet;
        [SerializeField] private FaceUIBehaviour _faceUISelected;
        [SerializeField] private Button _button;
        
        private Face _face;
        
        public event Action<FaceUI> FaceSelected;

        public Face Face => _face;

        public void SetFace(Face face, bool hover = false, bool isSelected = false)
        {
            _face = face;

            if (_face == null)
            {
                _icon.sprite = null;
                if (_description != null)
                {
                    _description.text = "Dice face is empty";
                }
                _sceneryValue.text = string.Empty;
                return;
            }

            _icon.sprite = _face.Sprite;
            _shadow.effectColor = _face.ShadowColor;
            if (_face.CurrentReward == 0)
            {
                _sceneryValue.text = string.Empty;
            }
            else
            {
                _sceneryValue.text = string.Format(_textFormat, _face.CurrentReward.ToString());
            }
            
            if (_description != null)
            {
                _description.text = _face.Description;
            }

            if (_button != null)
            {
                _button.interactable = true;
            }

            if(_faceUIHover == null)
                return;
            
            if (hover)
            {
                _faceUIHover.DoBehaviour();
            }
            else if(isSelected)
            {
                _faceUISelected.DoBehaviour();
            }
            else
            {
                _faceUISet.DoBehaviour();
            }
        }

        public void SetEmpty()
        {
            _button.interactable = false;
            _faceUIEmpty.DoBehaviour();
        }

        public void Clicked()
        {
            FaceSelected?.Invoke(this);
            _faceUISelected.DoBehaviour();
        }
    }
}