using System;
using UIGeneric;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _loadGameButton;
        [SerializeField] private ButtonAnimations _buttonAnimations;
        [SerializeField] private SaveManager _saveManager;

        private void Start()
        {
            _loadGameButton.interactable = _saveManager.HasSaveData;
            _buttonAnimations.SetEnable(_saveManager.HasSaveData);
        }

        public void LoadNewGame()
        {
            SceneManager.LoadScene(1);
        }

        public void LoadLastGame()
        {
            LoadManager.Instance.LoadLastGame();
        }
    }
}