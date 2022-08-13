using System;
using System.Collections.Generic;
using UIGeneric;
using UnityEngine;
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
        [SerializeField] private Button _accept;
        [SerializeField] private CanvasGroup _acceptButtonCanvasGroup;
        [SerializeField] private TimeManager _timeManager;

        private List<DieUI> _diceUI = new List<DieUI>();
        private DieUI _currentSelected;

        private Face _selectingFace;

        private void Awake()
        {
            _diceInventory.NewFaceAdded += ShowPopup;
            _chooseFacePopup.FaceSelected += FaceSelected;
            gameObject.SetActive(false);
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
            _accept.interactable = false;
            _acceptButtonCanvasGroup.alpha = 0.3f;
            _currentSelected = null;
            gameObject.SetActive(true);
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
            _accept.interactable = false;
            _acceptButtonCanvasGroup.alpha = 0.3f;
        }

        private void FaceSelected()
        {
            _accept.interactable = true;
            _acceptButtonCanvasGroup.alpha = 1f;
        }

        private void ClosePopup()
        {
            _timeManager.ResumeGame(GetHashCode());
            gameObject.SetActive(false);
        }

        public void Cancel()
        {
            _selectingFace.ToDestroy();
            _diceInventory.DiscardedNewFace();
            ClosePopup();
        }

        public void AcceptDice()
        {
            _chooseFacePopup.AcceptChange();
            ClosePopup();
        }
    }
}