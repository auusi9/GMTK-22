using System;
using UnityEngine;

namespace CityBuilder
{
    public class CityMap : MonoBehaviour
    {
        [SerializeField] private int _x, _y;
        [SerializeField] private CityTile _tilePrefab;
        
        private void Start()
        {
            for (int x = 0; x < _x; x++)
            {
                for (int y = 0; y < _y; y++)
                {
                    
                }
            }    
        }
    }
}