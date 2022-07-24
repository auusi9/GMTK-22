using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    public class ChooseFacePopup : MonoBehaviour
    {
        [SerializeField] private FaceUI _faceUI;
        [SerializeField] private FaceUI[] _currentFaces;
        [SerializeField] private Button _accept;
        [SerializeField] private ChooseDicePopup _chooseDicePopup;
        [SerializeField] private DiceInventory _diceInventory;

        private int _currentIndexSelected = -1;
        private Die _die;
        private Face _newFace;

        private void Start()
        {
            foreach (var faceUI in _currentFaces)
            {
                faceUI.FaceSelected += FaceSelected;
            }
        }

        private void OnDestroy()
        {
            foreach (var faceUI in _currentFaces)
            {
                faceUI.FaceSelected -= FaceSelected;
            }        
        }

        public void ShowPopup(Die die, Face newFace)
        {
            Time.timeScale = 0f;
            
            _newFace = newFace;
            _die = die;
            _faceUI.SetFace(newFace);

            for (int i = 0; i < _currentFaces.Length; i++)
            {
                _currentFaces[i].SetFace(die.Faces[i]);
            }

            _accept.interactable = false;
            gameObject.SetActive(true);
        }

        private void ClosePopup()
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }

        public void AcceptChange()
        {
            Face face = _die.AddFace(_newFace, _currentIndexSelected);
            _diceInventory.AcceptedNewFace(face);
            ClosePopup();
        }

        public void GoBack()
        {
            ClosePopup();
            _chooseDicePopup.ShowPopup(_newFace);
        }

        private void FaceSelected(FaceUI selectedFace)
        {
            if (_currentIndexSelected >= 0)
            {
                _currentFaces[_currentIndexSelected].SetFace(_die.Faces[_currentIndexSelected]);     
            }
            
            for (int i = 0; i < _currentFaces.Length; i++)
            {
                if (_currentFaces[i] == selectedFace)
                {
                    _currentIndexSelected = i;
                }
            }
            
            _currentFaces[_currentIndexSelected].SetFace(_newFace);
            _accept.interactable = true;
        }
    }
}