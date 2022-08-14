using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace UIGeneric
{
    public class FancyEnable : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        private Vector3 _originalPos = new Vector3(0f, 0f, 0f);
        private Vector3 _distance = new Vector3(0f, 15f, 0f);
        private float _animDuration = 0.15f;

        private TweenerCore<Vector3, Vector3, VectorOptions> _positionTween;
        private TweenerCore<float, float, FloatOptions> _canvasTween;
        
        private void OnEnable()
        {
            _originalPos = transform.localPosition;
            transform.localPosition = _originalPos - _distance;
            _canvasGroup.alpha = 0f;
            
            _positionTween = DOTween.To(() => transform.localPosition, x => transform.localPosition = x, _originalPos, _animDuration).SetUpdate(true);
            _canvasTween = DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1f, _animDuration * 2).SetUpdate(true);
        }

        private void OnDisable()
        {
            transform.localPosition = _originalPos;
            _canvasGroup.alpha = 0f;
            KillPositionTween();
            KillCanvasTween();
        }

        private void KillPositionTween()
        {
            if (_positionTween != null)
            {
                DOTween.Kill(_positionTween);
                _positionTween = null;
            }
        }

        private void KillCanvasTween()
        {
            if (_canvasTween != null)
            {
                DOTween.Kill(_canvasTween);
                _canvasTween = null;
            }
        }
    }
}