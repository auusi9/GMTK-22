using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using Resources;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dice
{
    public class DieUI : MonoBehaviour, IPointerMoveHandler, IPointerExitHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _selectedUI;
        [SerializeField] private Shadow _shadow;
        [SerializeField] private TextMeshProUGUI _sceneryValue;
        [SerializeField] private string _textFormat = "x{0}";
        [SerializeField] private List<FaceUI> _facesInfo;
        [SerializeField] private RectTransform _containerRectTrans;
        [SerializeField] private float _rollTime = 1f;
        [SerializeField] private float _strength = -0.5f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _randomness = 30f;

        private Die _die;

        public event Action<DieUI> DieSelected;
        public event Action<DieUI> DieStartHovered;
        public event Action<DieUI> DieStopHover;

        public Die Die => _die;

        private Face _currentFace;

        public void SetDie(Die die)
        {
            if (_die != null)
                _die.NewFace -= SetFacesInfo;
            
            _die = die;
            _die.NewFace += SetFacesInfo;
            
            SetFacesInfo();
            SwapFace(_die.GetFirstFace());
            SetDeselected();
        }

        private void OnDestroy()
        {
            if (_die != null)
                _die.NewFace -= SetFacesInfo;
        }

        private void SetFacesInfo()
        {
            if(_facesInfo == null)
                return;

            for (var i = 0; i < _facesInfo.Count; i++)
            {
                var face = _facesInfo[i];
                face.SetFace(_die.Faces[i]);
                SetFace(_die, i);
            }
        }
        
        private void SetFace(Die die, int i)
        {
            if (die.Faces[i] == null)
            {
                _facesInfo[i].SetFace(die.GetDefaultFace());
            }
            else
            {
                _facesInfo[i].SetFace(die.Faces[i]);
            }
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
            SetFacesInfo();
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
            _containerRectTrans.DOShakeScale(_rollTime, new Vector3(_strength, _strength, _strength), _vibrato, _randomness, true).SetUpdate(true);
        }

        public void SetDeselected()
        {
            _selectedUI.gameObject.SetActive(false);
            _containerRectTrans.DOShakeScale(_rollTime, new Vector3(_strength, _strength, _strength), _vibrato, _randomness, true).SetUpdate(true);
        }

        public void Clicked()
        {
            DieSelected?.Invoke(this);
        }

        public void ChangeFace()
        {
            SwapFace(_die.GetRandomFace(_currentFace));
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            DieStartHovered?.Invoke(this);
            SetFacesInfo();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DieStopHover?.Invoke(this);
        }
    }
}