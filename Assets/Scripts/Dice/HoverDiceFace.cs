using UnityEngine;
using UnityEngine.EventSystems;

namespace Dice
{
    [RequireComponent(typeof(FaceUI))]
    public class HoverDiceFace : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private FaceUI _faceUI;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_faceUI != null && _faceUI.Face != null && _faceUI.Face._building != null)
            {
                _faceUI.Face._building.Hover();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_faceUI != null && _faceUI.Face != null && _faceUI.Face._building != null)
            {
                _faceUI.Face._building.StopHover();
            }
        }
    }
}