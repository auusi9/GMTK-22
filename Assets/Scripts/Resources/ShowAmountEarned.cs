using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Resources
{
    public class ShowAmountEarned : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _color;
        [SerializeField] private string _textFormat = "+{0}";
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Show(int amount, Resource resource)
        {
            gameObject.SetActive(true);
            _text.text = String.Format(_textFormat, amount.ToString());
            _icon.sprite = resource.Sprite;
            _color.color = resource.ShadowColor;
        }
    }
}