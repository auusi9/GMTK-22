using System.Collections.Generic;
using UIGeneric;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CityBuilder
{
    public class MovingBuilding : DraggableObject
    {
        [SerializeField] private Building _building;
        
        private GraphicRaycaster _graphicRaycaster;
        
        public void Configure(Canvas canvas, RectTransform parent, GraphicRaycaster graphicRaycaster)
        {
            base.Configure(canvas, parent);
            _graphicRaycaster = graphicRaycaster;
        }
        
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            
            var results = new List<RaycastResult>();
            _graphicRaycaster.Raycast(eventData, results);

            CityTile currentTile = null;

            foreach (var hit in results)
            {
                var slot = hit.gameObject.GetComponent<CityTile>();
                if (slot)
                {
                    currentTile = slot;
                    break;
                }
            }

            if (currentTile == null)
            {
                Destroy(gameObject);
                return;
            }
            
            transform.SetParent(currentTile.transform);
            transform.localPosition = Vector3.zero;
        }
    }
}
