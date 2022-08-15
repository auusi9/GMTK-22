using System;
using TMPro;
using UnityEngine;

namespace Resources
{
    public class ShowAmountEarned : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private string _textFormat = "+{0}";
        
        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Show(int amount)
        {
            gameObject.SetActive(true);
            _text.text = String.Format(_textFormat, amount.ToString());
        }
    }
}