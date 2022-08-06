using System;
using UnityEngine;

namespace UIGeneric
{
    public class ObjectInsideCanvas : MonoBehaviour
    {
        [SerializeField] private GridCanvas _gridCanvas;

        private void OnEnable()
        {
            RectTransform rectTransform = transform as RectTransform;
            
            if(rectTransform == null)
                return;

            Vector3[] fourCornersArray = new Vector3[4];
            _gridCanvas.Parent.GetWorldCorners(fourCornersArray);

            Vector3 pos = rectTransform.position;
            
            Vector3[] fourCornersArrayObject = new Vector3[4];
            rectTransform.GetWorldCorners(fourCornersArrayObject);

            float halfSizeX = (fourCornersArrayObject[2].x - fourCornersArrayObject[0].x) / 2f;
            float halfSizeY = (fourCornersArrayObject[2].y - fourCornersArrayObject[0].y) / 2f;

            pos.x = Mathf.Clamp(pos.x, fourCornersArray[0].x + halfSizeX, fourCornersArray[2].x - halfSizeX);
            pos.y = Mathf.Clamp(pos.y, fourCornersArray[0].y + halfSizeY, fourCornersArray[2].y - halfSizeY);

            rectTransform.position = pos;
        }
    }
}