using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace UIGeneric
{
    public class ButtonAnimations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _targetTransform;
        [SerializeField] private Image _targetImage;
        [SerializeField] private CanvasGroup _canvasGroup;
        [Header("Hover Animation")]
        [SerializeField] private float _hoverAnimDuration = 0.1f;
        [SerializeField] private float _scaleAmount = 1.1f;
        [Header("On Pointer Down Animation")]
        [SerializeField] private float _pointerDownDuration = 0.1f;
        [SerializeField] private float _pointerDownScale = 0.9f;
        [Header("Disabled")]
        [SerializeField] private bool _enabled = true;

        private void OnEnable()
        {
            _targetTransform.localScale = Vector3.one;
            _targetImage.fillAmount = 0f;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AnimFill(_scaleAmount, 1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            AnimFill(1f, 0f);
        }

        private void AnimFill(float finalScale, float finalFill)
        {
            _targetTransform.DOScale(finalScale, _hoverAnimDuration).SetUpdate(true);
            _targetImage.DOFillAmount(finalFill, _hoverAnimDuration * 1.4f).SetUpdate(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _targetTransform.DOScale(_pointerDownScale, _pointerDownDuration).SetUpdate(true);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _targetTransform.DOScale(1f, _pointerDownDuration * 1.5f).SetEase(Ease.OutBack).SetUpdate(true);
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