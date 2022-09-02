using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace UIGeneric
{
    public class AnimScaleOnClick : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private float _punchDuration = 0.15f;
        [SerializeField] private float _punchStrenght = -0.2f;
        [SerializeField] private int _punchVibrato = 25;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            ResetScale();
            _transform.DOShakeScale(_punchDuration, new Vector3(_punchStrenght, _punchStrenght, _punchStrenght), _punchVibrato, 0, true).
                SetUpdate(true).OnComplete(() => ResetScale());
        }

        private void ResetScale()
        {
            _transform.localScale = Vector3.one;
        }
    }
}