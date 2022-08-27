using System;
using System.Collections;
using System.Collections.Generic;
using Dice;
using Dice.FaceUIBehaviours;
using Resources;
using TMPro;
using UIGeneric;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Workers
{
    public class EndOfNightPopup : MonoBehaviour
    {
        [SerializeField] private DayNight _dayNight;
        [SerializeField] private Resource _folks;
        [SerializeField] private Resource _food;
        [SerializeField] private TextMeshProUGUI _foodText;
        [SerializeField] private TextMeshProUGUI _folksText;
        [SerializeField] private TextMeshProUGUI _requiredFoodText;
        [SerializeField] private GameObject _workersDied;
        [SerializeField] private GameObject _noWorkersDied;
        [SerializeField] private GameObject _gameover;
        [SerializeField] private TextMeshProUGUI _workersDiedText;
        [SerializeField] private UIBehaviour _workerDiedColorBlink;
        [SerializeField] private Image _bg;
        [SerializeField] private Material _normalPolkadot;
        [SerializeField] private Material _redPolkadot;
        [SerializeField] private float _timeBetweenWorkers;
        [SerializeField] private Button _feedButton;
        [SerializeField] private Button _nextDay;
        [SerializeField] private TextMeshProUGUI _nextDayText;
        [SerializeField] private string _requiredFoodDescription = "x{0} Food required";
        [SerializeField] private string _townFolksHaveDied = "x{0} Townfolks have died";
        [SerializeField] private string _skipDescription = "Skip";
        [SerializeField] private string _nextDayDescription = "Next day";
        [SerializeField] private string _mainMenu = "Main menu";
        [SerializeField] private TimeManager _timeManager;

        private int _deadPeople = 0;
        private bool _playingAnimation = false;
        private Coroutine _animationCoroutine;
        
        private void Awake()
        {
            _dayNight.DayFinished += DayFinished;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _dayNight.DayFinished -= DayFinished;
        }

        private void DayFinished()
        {
            _timeManager.PauseGame(GetHashCode());

            gameObject.SetActive(true);
            _nextDay.gameObject.SetActive(false);
            _feedButton.gameObject.SetActive(true);

            _deadPeople = _dayNight.WorkersToRemove(out int neededFood);
            _folksText.text = _folks.Value.ToString();
            _foodText.text = _food.Value.ToString();
            _requiredFoodText.text = string.Format(_requiredFoodDescription, neededFood.ToString());
            _workersDied.SetActive(false);
            _noWorkersDied.SetActive(false);
            _gameover.SetActive(false);
            _bg.material = neededFood > _food.Value ? _redPolkadot : _normalPolkadot;
        }

        public void Feed()
        {
            _feedButton.gameObject.SetActive(false);
            _nextDay.gameObject.SetActive(true);
            _nextDayText.text = _skipDescription;
            
            _playingAnimation = true;
            _animationCoroutine = StartCoroutine(StartFeedAnimation());
        }

        private IEnumerator StartFeedAnimation()
        {
            int folks = _folks.Value;
            int food = _food.Value;
            int deadFolks = 0;
            for (int i = 0; i < folks; i++)
            {
                food -= _dayNight.FoodXFolk;

                if (food < 0)
                {
                    food = 0;
                    deadFolks++;
                    _workersDiedText.text = string.Format(_townFolksHaveDied, deadFolks.ToString());
                    _folksText.text = (_folks.Value - deadFolks).ToString();
                    _workerDiedColorBlink.DoBehaviour();
                    _workersDied.SetActive(true);
                }
                
                _foodText.text = food.ToString();
                yield return new WaitForSecondsRealtime(_timeBetweenWorkers);
            }

            AnimationFinished();
        }

        private void AnimationFinished()
        {
            _playingAnimation = false;
            
            if (_deadPeople > 0)
            {
                _workersDiedText.text = string.Format(_townFolksHaveDied, _deadPeople.ToString());
                _workersDied.SetActive(true);
            }
            else
            {
                _noWorkersDied.SetActive(true);
            }
            
            _dayNight.RemoveWorkers();
            _foodText.text = _food.Value.ToString();
            _folksText.text = _folks.Value.ToString();

            if (_folks.Value > 0)
            {
                _nextDayText.text = _nextDayDescription;
            }
            else
            {
                _nextDayText.text = _mainMenu;
                _noWorkersDied.SetActive(false);
                _gameover.SetActive(true);
                _workersDied.SetActive(false);
            }
        }

        public void AceiteButton()
        {
            if (_playingAnimation)
            {
                StopCoroutine(_animationCoroutine);
                AnimationFinished();
                return;
            }
            
            if (_folks.Value > 0)
            {
                AcceptButton();
            }
            else
            {
                ClosePopup();
                SceneManager.LoadScene(0);
            }
        }

        private void AcceptButton()
        {
            _dayNight.StartNewDay();
            ClosePopup();
        }

        private void ClosePopup()
        {
            _timeManager.ResumeGame(GetHashCode());
            gameObject.SetActive(false);
        }
    }
}