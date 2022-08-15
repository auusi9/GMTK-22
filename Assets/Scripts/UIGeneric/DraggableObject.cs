using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UIGeneric
{
    public class DraggableObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Shadow _shadow;
        [SerializeField] private float _scale = 1.0f;
        [SerializeField] protected GridCanvas _gridCanvas;

        public event Action RightClickEvent;
        
        private Vector3 _moveOffset;

        public void OnBeginDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_gridCanvas.Canvas.transform as RectTransform, eventData.position, _gridCanvas.Canvas.worldCamera, out Vector2 localPoint);
            _moveOffset = transform.position - _gridCanvas.Canvas.transform.TransformPoint(localPoint);
            OnDrag(eventData);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_gridCanvas.Canvas.transform as RectTransform, eventData.position, _gridCanvas.Canvas.worldCamera, out Vector2 localPoint);
            transform.position = _gridCanvas.Canvas.transform.TransformPoint(localPoint) + _moveOffset;
            
            Vector3[] fourCornersArray = new Vector3[4];
            _gridCanvas.Parent.GetWorldCorners(fourCornersArray);

            Vector3 pos = _rectTransform.position;

            pos.x = Mathf.Clamp(pos.x, fourCornersArray[0].x, fourCornersArray[2].x);
            pos.y = Mathf.Clamp(pos.y, fourCornersArray[0].y, fourCornersArray[2].y);
            _rectTransform.position = pos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            _shadow.enabled = false;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                RightClickEvent?.Invoke();
                PointerHandler.Instance.PanelStoppedHovering(GetHashCode());
                return;
            }
            
            transform.localScale = new Vector3(_scale, _scale, 1.0f);
            _shadow.enabled = true;
            transform.SetAsLastSibling();
            PointerHandler.Instance.PanelClicked(GetHashCode());
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _shadow.enabled = false;
            PointerHandler.Instance.PanelReleased(GetHashCode());
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerHandler.Instance.PanelHovered(GetHashCode());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerHandler.Instance.PanelStoppedHovering(GetHashCode());
        }
    }
}