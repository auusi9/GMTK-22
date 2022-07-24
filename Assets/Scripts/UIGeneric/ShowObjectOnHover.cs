using System;
using Dice;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIGeneric
{
    public class ShowObjectOnHover : MonoBehaviour, IPointerMoveHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _gameObject;

        private void OnEnable()
        {
            _gameObject.SetActive(false);
        }
        
        public void OnPointerMove(PointerEventData eventData)
        {
            _gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _gameObject.SetActive(false);
        }
    }
}