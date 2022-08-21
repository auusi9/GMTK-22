using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class LoadManager : MonoBehaviour
    {
        [SerializeField] private SaveManager _saveManager;
        
        public static LoadManager Instance = null;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }

        public void LoadLastGame()
        {
            StartCoroutine(LoadSceneAndLoadData());
        }

        private IEnumerator LoadSceneAndLoadData()
        {
            yield return SceneManager.LoadSceneAsync(1);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            
            _saveManager.LoadData();
        }

        private void OnApplicationQuit()
        {
            if (SceneManager.GetActiveScene().buildIndex > 0)
            {
                _saveManager.SaveData();
            }
        }
    }
}