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
        [SerializeField] private FaceUI _faceUI;
        [SerializeField] private Transform _gridParent;
        [SerializeField] private DiceInventory _diceInventory;
        [SerializeField] private ChooseFacePopup _chooseFacePopup;
        [SerializeField] private Button _selectDie;
        [SerializeField] private TimeManager _timeManager;

        private List<DieUI> _diceUI = new List<DieUI>();
        private DieUI _currentSelected;

        private Face _selectingFace;

        private void Awake()
        {
            _diceInventory.NewFaceAdded += ShowPopup;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
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

            _selectingFace = newFace;
            _faceUI.SetFace(newFace);
            _selectDie.interactable = false;
            _currentSelected = null;
            gameObject.SetActive(true);
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
            _selectDie.interactable = true;
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

        public void SelectDie()
        {
            ClosePopup();
            _chooseFacePopup.ShowPopup(_currentSelected.Die, _selectingFace);
        }
    }
}