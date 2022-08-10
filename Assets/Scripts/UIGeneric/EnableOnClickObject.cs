using UnityEngine;
using UnityEngine.EventSystems;

namespace UIGeneric
{
    public class EnableOnClickObject : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private GameObject _popupToEnable;

        public void OnPointerDown(PointerEventData eventData)
        {
            _popupToEnable.gameObject.SetActive(true);
        }
    }
}