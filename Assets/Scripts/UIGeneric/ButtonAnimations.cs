using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace UIGeneric
{
    public class ButtonAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [SerializeField] private RectTransform _targetTransform;
        [SerializeField] private Image _targetImage;
        [SerializeField] private CanvasGroup _canvasGroup;
        [Header("Hover Animation")]
        [SerializeField] private float _animDuration = 0.1f;
        [SerializeField] private float _scaleAmount = 1.1f;
        [Header("On Pointer Down Animation")]
        [SerializeField] private float _punchStrenght = 10f;
        [SerializeField] private int _punchVibrato = 10;
        [SerializeField] private float _punchElasticity = 1.0f;
        [Header("Disabled")]
        [SerializeField] private bool _enabled = true;

        private void OnEnable()
        {
            _targetTransform.localScale = Vector3.one;
            _targetImage.fillAmount = 0f;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _targetTransform.DOScale(_scaleAmount, _animDuration).SetUpdate(true);
            _targetImage.DOFillAmount(1f, _animDuration * 1.5f).SetUpdate(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _targetTransform.DOScale(1f, _animDuration).SetUpdate(true);
            _targetImage.DOFillAmount(0f, _animDuration * 1.5f).SetUpdate(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _targetTransform.DOPunchScale(new Vector3(_punchStrenght, _punchStrenght, _punchStrenght),
                    _animDuration, _punchVibrato, _punchElasticity).SetUpdate(true);
            }
        }

        public void SetEnable(bool enable)
        {
            if (_enabled == enable)
            {
                return;
            }

            _enabled = enable;
            
            if(_enabled == true)
            {
                SetCanvasGroup(true, 1f);
            }
            else
            {
                SetCanvasGroup(false, 0.3f);
            }
        }

        private void SetCanvasGroup(bool enable, float newAlpha)
        {
            _canvasGroup.alpha = newAlpha;
            _canvasGroup.interactable = enable;
            _canvasGroup.blocksRaycasts = enable;
        }
    }
}