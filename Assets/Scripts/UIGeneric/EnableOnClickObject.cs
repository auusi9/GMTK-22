using UnityEngine;
using UnityEngine.EventSystems;

namespace UIGeneric
{
    public class EnableOnClickObject : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private GameObject _popupToEnable;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }      
            _popupToEnable.gameObject.SetActive(true);
        }
    }
}