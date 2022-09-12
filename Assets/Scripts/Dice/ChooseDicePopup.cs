using System;
using System.Collections.Generic;
using UIGeneric;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Dice
{
    public class ChooseDicePopup : MonoBehaviour
    {
        [SerializeField] private DieUI _dieUIPrefab;
        [SerializeField] private NewFaceArea _faceUI;
        [SerializeField] private Transform _gridParent;
        [SerializeField] private DiceInventory _diceInventory;
        [SerializeField] private ChooseFacePopup _chooseFacePopup;
        [SerializeField] private ButtonAnimations _newAcceptButton;
        [SerializeField] private TimeManager _timeManager;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Transform _popupTransform;
        [SerializeField] private float _popupAnimDuration = 0.13f;
        [SerializeField] private float _popupInitialScale = 0.4f;
        [SerializeField]private float _closeAnimDelay = 0.075f;

        private List<DieUI> _diceUI = new List<DieUI>();
        private DieUI _currentSelected;

        private Face _selectingFace;

        private void Awake()
        {
            _diceInventory.NewFaceAdded += ShowPopup;
            _chooseFacePopup.FaceSelected += FaceSelected;
            gameObject.SetActive(false);
            _canvasGroup.interactable = false;
        }

        private void OnDestroy()
        {
            _chooseFacePopup.FaceSelected -= FaceSelected;
            _diceInventory.NewFaceAdded -= ShowPopup;
        }

        public void ShowPopup(Face newFace)
        {
            _timeManager.PauseGame(GetHashCode());
            if (_diceUI.Count < _diceInventory.Dice.Count)
            {
                int extraUI = _diceInventory.Dice.Count - _diceUI.Count;
                for (int i = 0; i < extraUI; i++)
                {
                    DieUI dieUI = Instantiate(_dieUIPrefab, _gridParent);
                    dieUI.DieSelected += DieSelected;
                    dieUI.DieStartHovered += DieStartHover;
                    dieUI.DieStopHover += DieStopHover;
                    _diceUI.Add(dieUI);
                }
            }

            for (var i = 0; i < _diceUI.Count; i++)
            {
                if (i < _diceInventory.Dice.Count)
                {
                    var die = _diceInventory.Dice[i];
                    _diceUI[i].SetDie(die);
                    _diceUI[i].gameObject.SetActive(true);
                }
                else
                {
                    _diceUI[i].gameObject.SetActive(false);
                }
            }

            _chooseFacePopup.SetEmpty();

            _selectingFace = newFace;
            _faceUI.SetFace(newFace);
            _newAcceptButton.SetEnable(false);
            _currentSelected = null;
            gameObject.SetActive(true);
            
            _canvasGroup.alpha = 0f;
            _popupTransform.localScale = new Vector3(_popupInitialScale, _popupInitialScale, _popupInitialScale);
            OpenPopupAnim();
        }

        private void DieStopHover(DieUI dieUI)
        {
            if (_currentSelected == dieUI)
            {
                return;
            }

            if (_currentSelected == null)
            {
                _chooseFacePopup.SetEmpty();
                return;
            }
            
            _chooseFacePopup.SetDice(_currentSelected.Die, _selectingFace, false);
        }

        private void DieStartHover(DieUI dieUI)
        {
            if (_currentSelected == dieUI)
            {
                return;
            }
            
            _chooseFacePopup.SetDice(dieUI.Die, _selectingFace, true);
        }

        private void DieSelected(DieUI dieUI)
        {
            if (_currentSelected == dieUI)
            {
                return;
            }

            if (_currentSelected != null)
            {
                _currentSelected.SetDeselected();
            }

            _currentSelected = dieUI;
            _currentSelected.SetSelected();
            _chooseFacePopup.ResetPopup();
            _chooseFacePopup.SetDice(_currentSelected.Die, _selectingFace, false);
            _newAcceptButton.SetEnable(false);
        }

        private void FaceSelected()
        {
            _newAcceptButton.SetEnable(true);
        }

        private void OpenPopupAnim()
        {
            _canvasGroup.DOFade(1f, _popupAnimDuration * 1.5f).SetUpdate(true);
            _popupTransform.DOScale(1f, _popupAnimDuration * 2f).SetEase(Ease.OutBack).SetUpdate(true).OnComplete(() => _canvasGroup.interactable = true);
        }

        private void ClosePopupAnim()
        {
            _canvasGroup.DOFade(0f, _popupAnimDuration).SetUpdate(true).SetDelay(_closeAnimDelay);
            _popupTransform.DOScale(_popupInitialScale, _popupAnimDuration).SetUpdate(true).SetEase(Ease.InCubic).SetDelay(_closeAnimDelay).OnComplete(()=>ClosePopup());
        }

        private void ClosePopup()
        {
            _canvasGroup.interactable = false;
            _timeManager.ResumeGame(GetHashCode());
            gameObject.SetActive(false);
        }

        public void Cancel()
        {
            _selectingFace.ToDestroy();
            _diceInventory.DiscardedNewFace();
            ClosePopupAnim();
        }

        public void AcceptDice()
        {
            _chooseFacePopup.AcceptChange();
            ClosePopupAnim();
        }
    }
}