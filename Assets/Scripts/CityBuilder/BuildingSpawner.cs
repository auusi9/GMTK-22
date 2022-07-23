using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CityBuilder
{
    public class BuildingSpawner : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private MovingBuilding _movingBuilding;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GraphicRaycaster _graphicRaycaster;

        private MovingBuilding _lastBuildingCreated;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            
            Vector3 position = _canvas.worldCamera.ScreenToWorldPoint(Input.mousePosition);
            var canvasTransform = _canvas.transform;
            position.z = canvasTransform.position.z;
            
            _lastBuildingCreated = Instantiate(_movingBuilding, position, _movingBuilding.transform.rotation, canvasTransform);
            _lastBuildingCreated.Configure(_canvas, (RectTransform)canvasTransform, _graphicRaycaster);
            _lastBuildingCreated.OnPointerDown(eventData);
            
            EventSystem.current.SetSelectedGameObject(_lastBuildingCreated.gameObject);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            _lastBuildingCreated?.OnDrag(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            _lastBuildingCreated?.OnBeginDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            _lastBuildingCreated?.OnEndDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            _lastBuildingCreated?.OnPointerUp(eventData);
            _lastBuildingCreated = null;
        }
    }
}