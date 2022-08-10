using System;
using TMPro;
using UnityEngine;

namespace CityBuilder.UI
{
    public class FactoryUI : MonoBehaviour
    {
        [SerializeField] private Factory _factory;
        [SerializeField] private string _textFormat = "Gives {0} {1} each {2} seconds x townfolk working on the building";
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        private void OnEnable()
        {
            FactoryInfo factoryInfo = _factory.GetFactoryInfo();
            _textMeshProUGUI.text = string.Format(_textFormat, factoryInfo.Coins.ToString(), factoryInfo.ResourceName.ToLower(),
                factoryInfo.Seconds);
        }
    }
}