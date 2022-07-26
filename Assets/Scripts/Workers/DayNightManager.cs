using System;
using System.Collections;
using UnityEngine;

namespace Workers
{
    public class DayNightManager : MonoBehaviour
    {
        [SerializeField] private DayNight _dayNight;

        private void Start()
        {
            _dayNight.StartDay += StartDay;
        }

        private void StartDay()
        {
            StartCoroutine(Day());
        }

        private IEnumerator Day()
        {
            float time = 0f;

            while (time < _dayNight.DayDuration)
            {
                yield return 0;
                time += Time.deltaTime;
                _dayNight.UpdateDayProgress(time);
            }
        }
    }
}