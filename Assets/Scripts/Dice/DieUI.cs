using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    public class DieUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _selectedUI;
        [SerializeField] private Shadow _shadow;
        [SerializeField] private TextMeshProUGUI _sceneryValue;
        [SerializeField] private string _textFormat = "x{0}";
        
        private Die _die;

        public event Action<DieUI> DieSelected;

        public Die Die => _die;

        private Face _currentFace;

        public void SetDie(Die die)
        {
            _die = die;
            SwapFace(_die.GetFirstFace());
            SetDeselected();
        }

        private void SetFaceInfo()
        {
            _icon.sprite = _currentFace.Sprite;
            _shadow.effectColor = _currentFace.ShadowColor;
            SetReward(_currentFace.CurrentReward);
        }

        private void SwapFace(Face newFace)
        {
            if(newFace == _currentFace)
                return;
            
            if (_currentFace != null)
            {
                _currentFace.Destroying -= FaceDestroyed;
                _currentFace.RewardChanged -= RewardChanged;
            }

            if (newFace == null)
                newFace = _die.GetDefaultFace();
            
            _currentFace = newFace;
            _currentFace.Destroying += FaceDestroyed;
            _currentFace.RewardChanged += RewardChanged;
            SetFaceInfo();
        }

        private void RewardChanged()
        {
            SetFaceInfo();
        }

        private void FaceDestroyed()
        {
            _currentFace.Destroying -= FaceDestroyed;
            _currentFace.RewardChanged -= RewardChanged;
            
            SwapFace(_die.GetRandomFace(_currentFace));
        }

        private void SetReward(int value)
        {
            if (value != 0)
            {
                _sceneryValue.text = string.Format(_textFormat, value);
            }
            else
            {
                _sceneryValue.text = string.Empty;
            }
        }

        public void SetFace(Face face)
        {
            if (face == null)
            {
                SwapFace(_die.GetDefaultFace());
                return;
            }
            
            SwapFace(face);
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
            SwapFace(_die.GetRandomFace(_currentFace));
        }
    }
}