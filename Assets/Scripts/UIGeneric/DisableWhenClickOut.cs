using UnityEngine;

namespace UIGeneric
{
    public class DisableWhenClickOut : MonoBehaviour
    {
        private bool _stillClicked = false;
        private void OnEnable()
        {
            
            if (Input.GetMouseButton(0))
            {
                _stillClicked = true;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButton(0) && _stillClicked)
            {
                return;
            }

            _stillClicked = false;
            HideIfClickedOutside();
        }

        private void HideIfClickedOutside() 
        {
            if (Input.GetMouseButton(0) && gameObject.activeSelf && 
                !RectTransformUtility.RectangleContainsScreenPoint(
                    transform as RectTransform, 
                    Input.mousePosition, 
                    Camera.main)) {
                gameObject.SetActive(false);
            }
        }
    }
}