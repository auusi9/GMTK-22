using System;
using System.Collections.Generic;
using Dice;
using Resources;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CityBuilder
{
    public class Market : MonoBehaviour
    {
        [SerializeField] private List<TradeablePair> _tradeablePairs = new List<TradeablePair>();
        [SerializeField] private MarketPair _marketPairPrefab;
        [SerializeField] private Transform _container;

        private List<MarketPair> _marketPairs = new List<MarketPair>();
        
        private void Start()
        {
            foreach (var tradeablePair in _tradeablePairs)
            {
                MarketPair marketPair = Instantiate(_marketPairPrefab, _container);
                marketPair.SetTradeablePair(tradeablePair);
                _marketPairs.Add(marketPair);
            }
        }
    }

    [Serializable]
    public class TradeablePair
    {
        [SerializeField] private Resource _firstResource;
        [SerializeField] private Resource _secondResource;
        
        [SerializeField] private int _amountFirstResource;
        [SerializeField] private int _amountSecondResource;

        public int GetPriceFirstResource()
        {
            return _amountFirstResource;
        }

        public int GetPriceSecondResource()
        {
            return _amountSecondResource;
        }

        public void ExchangeFirstForSecond()
        {
            _firstResource.RemoveResource(_amountFirstResource);
            _secondResource.AddResource(_amountSecondResource);
        }

        public void ExchangeSecondForFirst()
        {
            _secondResource.RemoveResource(_amountSecondResource);
            _firstResource.AddResource(_amountFirstResource);
        }

        public Resource GetFirstResource()
        {
            return _firstResource;
        }
        
        public Resource GetSecondResource()
        {
            return _secondResource;
        }
    }
}