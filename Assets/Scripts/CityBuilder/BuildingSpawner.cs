﻿using System;
using Resources;
using TMPro;
using UIGeneric;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CityBuilder
{
    public class BuildingSpawner : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Building _building;
        [SerializeField] private GridCanvas _canvas;
        [SerializeField] private ResourcePrice[] _resourcePrices;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Color _colorAvailable;
        [SerializeField] private Color _colorUnavailable;

        private Building _lastBuildingCreated;
        private bool _constructing = false;

        private void Awake()
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

            _title.text = _building.BuildingName;
            _description.text = _building.BuildingDescription;
            
            ResourceChanged(0);
        }

        private void OnDestroy()
        {
            foreach (var cost in _building.Cost)
            {
                cost.Resource.ResourceChanged -= ResourceChanged;
            }
        }

        private void Update()
        {
            if (_constructing)
            {
                PointerEventData eventData = (EventSystem.current.currentInputModule as StandaloneInputModuleCustom)?.GetLastPointerEventDataPublic(-1);
                
                if (Input.GetMouseButtonDown(0))
                {
                    OnEndDrag(eventData);
                    _constructing = false;
                    _lastBuildingCreated?.DraggableObject.OnPointerUp(eventData);
                    _lastBuildingCreated = null;
                    PointerHandler.Instance.PanelReleased(GetHashCode()); 
                    return;
                }

                if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                {
                    CancelConstruction();
                    return;
                }
                
                OnDrag(eventData);
            }
        }

        private void CancelConstruction()
        {
            _constructing = false;
            _lastBuildingCreated.DraggableObject.Released();
            Destroy(_lastBuildingCreated.gameObject);
            _lastBuildingCreated = null;
            PointerHandler.Instance.PanelReleased(GetHashCode());
            PointerHandler.Instance.PanelStoppedHovering(GetHashCode());
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
            _image.color = _colorAvailable;
            enabled = true;
        }

        private void UnAvailable()
        {
            _image.color = _colorUnavailable;
            enabled = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_constructing)
            {
                return;
            }
            
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }

            Vector3 position = _canvas.Canvas.worldCamera.ScreenToWorldPoint(Input.mousePosition);
            var canvasTransform = _canvas.Canvas.transform;
            position.z = canvasTransform.position.z;
            
            _lastBuildingCreated = Instantiate(_building, position, _building.transform.rotation, canvasTransform);
            _lastBuildingCreated.DraggableObject.OnPointerDown(eventData);

            EventSystem.current.SetSelectedGameObject(_lastBuildingCreated.gameObject);
            PointerHandler.Instance.PanelClicked(GetHashCode());
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
            if (eventData.button == PointerEventData.InputButton.Right )
            {
                return;
            }

            _constructing = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_lastBuildingCreated == null)
            {
                PointerHandler.Instance.PanelHovered(GetHashCode());
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerHandler.Instance.PanelStoppedHovering(GetHashCode());
        }
    }
}