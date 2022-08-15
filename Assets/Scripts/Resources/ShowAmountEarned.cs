using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Resources
{
    public class ShowAmountEarned : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        //[SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private string _textFormat = "+{0}";
        //[SerializeField] private Vector3 _distance;
        //[SerializeField] private float _duration;
        //[SerializeField] private Ease _ease;
        //[SerializeField] private float _newScale = 1.2f;

        private Vector3 _localPosition;

        private void Start()
        {
            _localPosition = transform.localPosition;
            gameObject.SetActive(false);
        }

        public void Show(int amount)
        {
            //transform.localPosition = _localPosition;
            //transform.localScale = Vector3.one;
            //_canvasGroup.alpha = 0f;
            gameObject.SetActive(true);
            _text.text = String.Format(_textFormat, amount.ToString());
            //transform.DOLocalMove(_localPosition + _distance, _duration * 0.5f).SetEase(_ease);
            //DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1f, _duration * 0.5f);
            //transform.DOScale(_newScale, _duration).SetEase(Ease.OutBounce).OnComplete(() => gameObject.SetActive(false));
        }
    }
}