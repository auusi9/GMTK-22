using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIGeneric
{
    public class ShowObjectOnHover : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private GameObject _gameObject;

        private void OnEnable()
        {
            _gameObject.SetActive(false);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _gameObject.SetActive(true);
        }
    }
}