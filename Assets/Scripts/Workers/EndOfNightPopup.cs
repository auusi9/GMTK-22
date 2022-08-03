using System;
using System.Collections;
using System.Collections.Generic;
using Dice;
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
        [SerializeField] private TextMeshProUGUI _requiredfoodText;
        [SerializeField] private TextMeshProUGUI _foodxFolkText;
        [SerializeField] private TextMeshProUGUI _folksText;
        [SerializeField] private TextMeshProUGUI _workersDying;
        [SerializeField] private TextMeshProUGUI _remainingFolks;
        [SerializeField] private TextMeshProUGUI _noWorkersDied;
        [SerializeField] private GameObject _line;
        [SerializeField] private Button _aceiteButton;
        [SerializeField] private Button _finishGameButton;
        [SerializeField] private string _workersDyingDescription = "{0} will die from starvation";
        [SerializeField] private string _workersRemainingDescription = "{0} folks remain";
        [SerializeField] private string _foodxFolkTextDescription = "x{0} FoodxFolk";
        [SerializeField] private float _timeBetweenFields = 0.1f;
        [SerializeField] private float _timeLerpValue = 0.5f;
        [SerializeField] private TimeManager _timeManager;
        
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

            _finishGameButton.gameObject.SetActive(false);
            _aceiteButton.gameObject.SetActive(false);
            gameObject.SetActive(true);
            List<Animation> animations = new List<Animation>();

            animations.Add(new Animation(_folksText.transform.parent.gameObject, _folksText, true, true, 0, _folks.Value, false, string.Empty));
            animations.Add(new Animation(_foodText.transform.parent.gameObject, _foodText,true, true, 0, _food.Value, false, string.Empty));
            animations.Add(new Animation(_foodxFolkText.transform.parent.gameObject, _foodxFolkText,true, false, 0, _dayNight.FoodXFolk, true, _foodxFolkTextDescription));
            animations.Add(new Animation(_line, null, false,false, 0, 0, false, string.Empty));
            
            int deadPeople = _dayNight.WorkersToRemove(out int neededFood);
            _noWorkersDied.gameObject.SetActive(false);
            _workersDying.gameObject.SetActive(false);

            animations.Add(new Animation(_requiredfoodText.transform.parent.gameObject, _requiredfoodText, true,true, _food.Value, neededFood, false, string.Empty));

            if (deadPeople > 0)
            {
                animations.Add(new Animation(_workersDying.gameObject, _workersDying,true, true, _folks.Value, deadPeople, true, _workersDyingDescription));
                animations.Add(new Animation(_remainingFolks.gameObject, _remainingFolks,true, true, _folks.Value, _folks.Value - deadPeople, true, _workersRemainingDescription));
            }
            else
            {
                animations.Add(new Animation(_noWorkersDied.gameObject, null,false, false, 0, _food.Value, false, string.Empty));
            }

            foreach (var anim in animations)
            {
                anim.Parent.gameObject.SetActive(false);
            }

            StartCoroutine(PlayAnimation(animations));
        }

        private IEnumerator PlayAnimation(List<Animation> animations)
        {
            foreach (var anim in animations)
            {
                anim.Parent.gameObject.SetActive(true);

                if (anim.HasValue)
                {
                    if (anim.HasAnimationValue)
                    {
                        float time = 0f;
                        while (_timeLerpValue > time)
                        {
                            int value = (int)Mathf.Lerp(anim.InitialValue, anim.FinalValue, time / _timeLerpValue);
                        
                            if(anim.HasCustomText)
                            {
                                anim.TextMeshProUGUI.text = string.Format(anim.CustomText, value);
                            }
                            else
                            {
                                anim.TextMeshProUGUI.text = value.ToString();
                            }

                            yield return new WaitForEndOfFrame();
                            time += Time.unscaledDeltaTime;
                        }
                    }
                
                    if(anim.HasCustomText)
                    {
                        anim.TextMeshProUGUI.text = string.Format(anim.CustomText, anim.FinalValue);
                    }
                    else
                    {
                        anim.TextMeshProUGUI.text = anim.FinalValue.ToString();
                    }
                }

                yield return new WaitForSecondsRealtime(_timeBetweenFields);
            }
            
            _dayNight.RemoveWorkers();

            if (_folks.Value <= 0)
            {
                _finishGameButton.gameObject.SetActive(true);
            }
            else
            {
                _aceiteButton.gameObject.SetActive(true);
            }
        }

        public void MainMenuButton()
        {
            SceneManager.LoadScene(0);
        }

        public void AcceptButton()
        {
            _dayNight.StartNewDay();
            gameObject.SetActive(false);
        }

        private void ClosePopup()
        {
            _timeManager.ResumeGame(GetHashCode());
        }
        
        private class Animation
        {
            public GameObject Parent;
            public TextMeshProUGUI TextMeshProUGUI;
            public bool HasValue;
            public bool HasAnimationValue;
            public int InitialValue;
            public int FinalValue;
            public bool HasCustomText;
            public string CustomText;

            public Animation(GameObject parent, TextMeshProUGUI textMeshProUGUI, bool hasValue, bool hasAnimationValue, int initialValue, int finalValue, bool hasCustomText, string customText)
            {
                Parent = parent;
                TextMeshProUGUI = textMeshProUGUI;
                HasValue = hasValue;
                HasAnimationValue = hasAnimationValue;
                InitialValue = initialValue;
                FinalValue = finalValue;
                HasCustomText = hasCustomText;
                CustomText = customText;
            }
        }
    }
}