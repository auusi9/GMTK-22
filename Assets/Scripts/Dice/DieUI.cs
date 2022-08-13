using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        [SerializeField] private TextMeshProUGUI _facesInfo;
        [SerializeField] private string _textFormatForFacesInfo = "x{0} {1} <br>";

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
            
            StringBuilder stringBuilder = new StringBuilder();
            IEnumerable<IGrouping<Resource, Face>> groupBy = _die.Faces.Where(x => x != null).GroupBy(x => x.Resource);

            int totalFaces = 0;
            foreach (var dieFace in groupBy)
            {
                List<Face> faces = dieFace.ToList();
                stringBuilder.Append(
                    string.Format(_textFormatForFacesInfo, faces.Count.ToString(), faces[0].Resource.ResourceName));
                totalFaces += faces.Count;
            }

            if (totalFaces < _die.Faces.Length)
            {
                stringBuilder.Append(
                    string.Format(_textFormatForFacesInfo, (_die.Faces.Length - totalFaces).ToString(), "Empty"));
            }

            _facesInfo.text = stringBuilder.ToString();
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

        public void OnPointerMove(PointerEventData eventData)
        {
            DieStartHovered?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DieStopHover?.Invoke(this);
        }
    }
}