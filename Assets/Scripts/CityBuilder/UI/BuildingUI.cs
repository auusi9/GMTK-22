using System;
using Dice;
using TMPro;
using UnityEngine;

namespace CityBuilder.UI
{
    public class BuildingUI : MonoBehaviour
    {
        [SerializeField] private Building _building;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _name.text = _building.BuildingName;
            _description.text = _building.BuildingDescription;
        }

        public void DestroyBuilding()
        {
            Destroy(_building.gameObject);
        }
    }
}