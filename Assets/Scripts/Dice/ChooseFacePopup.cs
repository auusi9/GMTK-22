using System;
using UIGeneric;
using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    public class ChooseFacePopup : MonoBehaviour
    {
        [SerializeField] private FaceUI[] _currentFaces;
        [SerializeField] private DiceInventory _diceInventory;
        [SerializeField] private FaceUI _replaceMessage;

        private int _currentIndexSelected = -1;
        private Die _die;
        private Face _newFace;

        public event Action FaceSelected;

        private void Start()
        {
            foreach (var faceUI in _currentFaces)
            {
                faceUI.FaceSelected += NewFaceSelected;
            }
        }

        private void OnDestroy()
        {
            foreach (var faceUI in _currentFaces)
            {
                faceUI.FaceSelected -= NewFaceSelected;
            }        
        }

        public void ResetPopup()
        {
            _currentIndexSelected = -1;
        }

        public void SetDice(Die die, Face newFace, bool hover)
        {
            _newFace = newFace;
            _die = die;

            for (int i = 0; i < _currentFaces.Length; i++)
            {
                if (_currentIndexSelected == i && !hover)
                {
                    _currentFaces[_currentIndexSelected].SetFace(_newFace, hover, true);
                }
                else
                {
                    SetFace(die, i, hover);
                }
            }
        }

        private void SetFace(Die die, int i, bool hover)
        {
            if (die.Faces[i] == null)
            {
                _currentFaces[i].SetFace(die.GetDefaultFace(), hover);
            }
            else
            {
                _currentFaces[i].SetFace(die.Faces[i], hover);
            }
        }

        public void AcceptChange()
        {
            Face face = _die.AddFace(_newFace, _currentIndexSelected);
            _diceInventory.AcceptedNewFace(face);
        }

        private void NewFaceSelected(FaceUI selectedFace)
        {
            if (selectedFace.Face != null && selectedFace.Face != _newFace && selectedFace.Face != _die.GetDefaultFace())
            {
                _replaceMessage.SetFace(selectedFace.Face);
                _replaceMessage.gameObject.SetActive(true);
            }
            else
            {
                _replaceMessage.gameObject.SetActive(false);
            }
            
            if (_currentIndexSelected >= 0)
            {
                SetFace(_die, _currentIndexSelected, false);
            }
            
            for (int i = 0; i < _currentFaces.Length; i++)
            {
                if (_currentFaces[i] == selectedFace)
                {
                    _currentIndexSelected = i;
                }
            }
            
            _currentFaces[_currentIndexSelected].SetFace(_newFace);
            
            FaceSelected?.Invoke();
        }

        public void SetEmpty()
        {
            foreach (var faceUI in _currentFaces)
            {
                faceUI.SetEmpty();
            }
        }
    }
}