using System;
using Dice;
using UnityEngine;
using UnityEngine.UI;

namespace CityBuilder.UI
{
    public class SceneryUI : MonoBehaviour
    {
        [SerializeField] private Scenery _scenery;
        [SerializeField] private Image _fill;

        private void OnEnable()
        {
            _scenery.SceneryAmountChanged += SceneryAmountChanged;
            _scenery.SceneryReset += SceneryReset;
        }

        private void OnDisable()
        {
            _scenery.SceneryAmountChanged -= SceneryAmountChanged;
            _scenery.SceneryReset -= SceneryReset;
        }

        private void SceneryReset()
        {
            _fill.fillAmount = _scenery.Percentage;
        }

        private void SceneryAmountChanged(Face face = null)
        {
            _fill.fillAmount = _scenery.Percentage;
        }
    }
}