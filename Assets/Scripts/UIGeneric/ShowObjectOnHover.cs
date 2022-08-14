using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace UIGeneric
{
    public class ShowObjectOnHover : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private CanvasGroup _canvasGroup;

        private Vector3 _originalPos = new Vector3(0f, 0f, 0f);
        private Vector3 _distance = new Vector3(0f, 15f, 0f);
        private float _animDuration = 0.15f;

        private void OnEnable()
        {
            _gameObject.SetActive(false);
            _originalPos = _gameObject.transform.localPosition;
            _gameObject.transform.localPosition = _originalPos - _distance;
            _canvasGroup.alpha = 0f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _gameObject.SetActive(false);
            DOTween.Kill("InfoMenuTransf");
            DOTween.Kill("InfoMenuCanvasGroup");
            _gameObject.transform.localPosition = _originalPos - _distance;
            _canvasGroup.alpha = 0f;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _gameObject.SetActive(true);
            DOTween.To(() => _gameObject.transform.localPosition, x => _gameObject.transform.localPosition = x, _originalPos, _animDuration).SetUpdate(true).SetId("InfoMenuTransf");
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1f, _animDuration * 2).SetUpdate(true).SetId("InfoMenuCanvasGroup");
        }
        private void OnDisable()
        {
            _gameObject.transform.localPosition = _originalPos;
            _canvasGroup.alpha = 0f;
            DOTween.Kill("InfoMenuTransf");
            DOTween.Kill("InfoMenuCanvasGroup");
        }
    }
}