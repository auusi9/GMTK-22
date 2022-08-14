using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Resources
{
    public class ShowAmountEarned : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private string _textFormat = "+{0}";
        [SerializeField] private Vector3 _endValue;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;

        private Vector3 _localPosition;

        private void Start()
        {
            _localPosition = transform.localPosition;
            gameObject.SetActive(false);
        }

        public void Show(int amount)
        {
            transform.localPosition = _localPosition;
            gameObject.SetActive(true);
            _text.text = String.Format(_textFormat, amount.ToString());
            transform.DOLocalMove(_endValue, _duration).SetEase(_ease).OnComplete(() => gameObject.SetActive(false));
        }
    }
}