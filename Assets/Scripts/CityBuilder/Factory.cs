using Resources;
using UnityEngine;

namespace CityBuilder
{
    public class Factory : MonoBehaviour
    {
        [SerializeField] private Building _building;
        [SerializeField] private Resource _gold;
        [SerializeField] private int _coins;
        [SerializeField] private float _seconds;

        private float _nextCoin = 0f;

        public FactoryInfo GetFactoryInfo()
        {
            return new FactoryInfo()
            {
                Coins = _coins,
                Seconds = _seconds,
                ResourceName = _gold.ResourceName
            };
        }
        
        private void Update()
        {
            float coinsCreated = _nextCoin;
            _nextCoin = 0f;
            foreach (var workerSpot in _building.WorkerSpots)
            {
                if (!workerSpot.Available)
                {
                    float efficency = workerSpot.Worker.Work();
                    coinsCreated += efficency * (_coins / _seconds * Time.deltaTime);
                }
            }

            _nextCoin = coinsCreated - Mathf.FloorToInt(coinsCreated);
            _gold.AddResource(Mathf.FloorToInt(coinsCreated));
        }
    }

    public class FactoryInfo
    {
        public int Coins;
        public float Seconds;
        public string ResourceName;
    }
}