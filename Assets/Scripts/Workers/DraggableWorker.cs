using System.Collections.Generic;
using CityBuilder;
using UIGeneric;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Workers
{
    public class DraggableWorker : DraggableObject
    {
        [SerializeField] private Worker _worker;
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            
            TrySetWorker(eventData);
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
    }
}