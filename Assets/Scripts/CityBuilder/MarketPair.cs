using System;
using Resources;
using UnityEngine;
using UnityEngine.UI;

namespace CityBuilder
{
    public class MarketPair : MonoBehaviour
    {
        [SerializeField] private ResourceStaticDisplay _firstResourceDisplay;
        [SerializeField] private ResourceStaticDisplay _secondResourceDisplay;
        [SerializeField] private Button _buyFirstResource;
        [SerializeField] private Button _buySecondResource;

        private TradeablePair _tradeablePair;

        public void BuyFirstResource()
        {
            _tradeablePair?.ExchangeSecondForFirst();
        }

        public void BuySecondResource()
        {
            _tradeablePair?.ExchangeFirstForSecond();
        }
        
        public void SetTradeablePair(TradeablePair tradeablePair)
        {
            _tradeablePair = tradeablePair;
            
            _tradeablePair.GetFirstResource().ResourceChanged += ResourceChanged;
            _tradeablePair.GetSecondResource().ResourceChanged += ResourceChanged;
            
            _firstResourceDisplay.SetInformation(_tradeablePair.GetFirstResource(), _tradeablePair.GetPriceFirstResource());
            _secondResourceDisplay.SetInformation(_tradeablePair.GetSecondResource(), _tradeablePair.GetPriceSecondResource());
        }

        private void ResourceChanged(int obj)
        {
            Resource firstResource = _tradeablePair.GetFirstResource();
            Resource secondResource = _tradeablePair.GetSecondResource();

            _buyFirstResource.interactable = secondResource.Value >= _tradeablePair.GetPriceSecondResource();
            _buySecondResource.interactable = firstResource.Value >= _tradeablePair.GetPriceFirstResource();
        }

        private void OnDestroy()
        {
            if(_tradeablePair == null)
                return;
            
            _tradeablePair.GetFirstResource().ResourceChanged -= ResourceChanged;
            _tradeablePair.GetSecondResource().ResourceChanged -= ResourceChanged;
        }
    }
}