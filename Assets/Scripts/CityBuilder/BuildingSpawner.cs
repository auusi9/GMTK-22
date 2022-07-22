using System;
using UnityEngine;

namespace CityBuilder
{
    public class BuildingSpawner : MonoBehaviour
    {
        [SerializeField] private MovingBuilding _movingBuilding;
        [SerializeField] private Canvas _canvas;

        private void Start()
        {
            MovingBuilding _building = Instantiate(_movingBuilding, transform);
            _building.Configure(_canvas, (RectTransform)transform);
        }
    }
}