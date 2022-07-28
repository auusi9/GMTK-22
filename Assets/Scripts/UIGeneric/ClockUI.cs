using System;
using UnityEngine;
using Workers;

namespace UIGeneric
{
    public class ClockUI : MonoBehaviour
    {
        [SerializeField] private DayNight _dayNight;
        [SerializeField] private RectTransform _rectTransform;


        private void Update()
        {
            _rectTransform.localRotation = Quaternion.Euler(0, 0, _dayNight.DayProgress * -360);
        }
    }
}