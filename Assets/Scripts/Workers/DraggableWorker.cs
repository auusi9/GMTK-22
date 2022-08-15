using System.Collections.Generic;
using CityBuilder;
using UIGeneric;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Workers
{
    public class DraggableWorker : DraggableObject
    {
        [SerializeField] private Worker _worker;

        private bool _moving = false;
        private bool _canPlace = false;
        
        private void Update()
        {
            if (_moving)
            {
                PointerEventData eventData = (EventSystem.current.currentInputModule as StandaloneInputModuleCustom)?.GetLastPointerEventDataPublic(-1);

                if (Input.GetMouseButtonDown(0) && _canPlace)
                {
                    OnEndDrag(eventData);
                    _moving = false;
                    _canPlace = false;
                    _gridCanvas.GraphicRaycaster.enabled = true;
                    TrySetWorker(eventData);
                    PointerHandler.Instance.PanelReleased(GetHashCode()); 
                    return;
                }

                if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                {
                    _moving = false;
                    _canPlace = false;
                    _gridCanvas.GraphicRaycaster.enabled = true;
                    _worker.CurrentSpot.SetWorker(_worker); 
                    return;
                }
                
                OnDrag(eventData);
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            
            if (_moving || eventData.button != PointerEventData.InputButton.Left )
            {
                return;
            }

            _gridCanvas.GraphicRaycaster.enabled = false;
            _moving = true;
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            
            if (_moving && eventData.button == PointerEventData.InputButton.Left )
            {
                _canPlace = true;
            }
        }

        private void TrySetWorker(PointerEventData eventData)
        {
            var results = new List<RaycastResult>();
            _gridCanvas.GraphicRaycaster.Raycast(eventData, results);

            WorkerSpot currentTile = null;

            foreach (var hit in results)
            {
                var slot = hit.gameObject.GetComponent<WorkerSpot>();
                if (slot && slot.Available)
                {
                    currentTile = slot;
                    slot.SetWorker(_worker);
                    break;
                }

                var building = hit.gameObject.GetComponent<Building>();

                if (building && building.HasAvailableSpot())
                {
                    currentTile = building.GetAvailableSpot();
                    currentTile.SetWorker(_worker);
                    break;
                }
            }

            if (currentTile == null)
            {
                _worker.CurrentSpot.SetWorker(_worker);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                return;
            }
            
            var results = new List<RaycastResult>();
            _gridCanvas.GraphicRaycaster.Raycast(eventData, results);

            WorkerSpot currentTile = null;

            foreach (var hit in results)
            {
                var slot = hit.gameObject.GetComponent<WorkerSpot>();
                if (slot && slot.Available)
                {
                    currentTile = slot;
                    _worker.transform.position = slot.transform.position;
                    break;
                }

                var building = hit.gameObject.GetComponent<Building>();

                if (building && building.HasAvailableSpot())
                {
                    currentTile = building.GetAvailableSpot();
                    _worker.transform.position = currentTile.transform.position; 
                    break;
                }
            }
        }
    }
}