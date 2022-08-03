using System;
using Resources;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CityBuilder
{
    public class BuildingSpawner : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Building _building;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private ResourcePrice[] _resourcePrices;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _unavailableAlpha = 0.5f;

        private Building _lastBuildingCreated;

        private void Start()
        {
            foreach (var cost in _building.Cost)
            {
                cost.Resource.ResourceChanged += ResourceChanged;
            }
        }

        private void OnEnable()
        {
            foreach (var resourcePrice in _resourcePrices)
            {
                resourcePrice.gameObject.SetActive(false);
            }
            
            int min = Mathf.Min(_resourcePrices.Length, _building.Cost.Length);
            
            for (var i = 0; i < min; i++)
            {
                _resourcePrices[i].SetBuildingCost(_building.Cost[i]);
            }
            
            ResourceChanged(0);
        }

        private void OnDestroy()
        {
            foreach (var cost in _building.Cost)
            {
                cost.Resource.ResourceChanged -= ResourceChanged;
            }
        }

        private void ResourceChanged(int obj)
        {
            if (_building.CanAfford())
            {
                Available();
            }
            else
            {
                UnAvailable();
            }
        }

        private void Available()
        {
            _canvasGroup.alpha = 1f;
            enabled = true;
        }

        private void UnAvailable()
        {
            _canvasGroup.alpha = _unavailableAlpha;
            enabled = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            
            Vector3 position = _canvas.worldCamera.ScreenToWorldPoint(Input.mousePosition);
            var canvasTransform = _canvas.transform;
            position.z = canvasTransform.position.z;
            
            _lastBuildingCreated = Instantiate(_building, position, _building.transform.rotation, canvasTransform);
            _lastBuildingCreated.DraggableObject.OnPointerDown(eventData);

            EventSystem.current.SetSelectedGameObject(_lastBuildingCreated.gameObject);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            _lastBuildingCreated?.DraggableObject.OnDrag(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            _lastBuildingCreated?.DraggableObject.OnBeginDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            _lastBuildingCreated?.DraggableObject.OnEndDrag(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            _lastBuildingCreated?.DraggableObject.OnPointerUp(eventData);
            _lastBuildingCreated = null;
        }
    }
}