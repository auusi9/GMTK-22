using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Resources
{
    public class ResourceDisplay : MonoBehaviour
    {
        [SerializeField] private Resource _resource;
        [SerializeField] private Image _image;
        [SerializeField] private Shadow _shadow;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private string _textFormat = "{0}";
        [SerializeField] private float _incrementXSecond = 5f;

        private float _currentValue = 0;
        
        private void OnEnable()
        {
            _resource.AddedResource += AddedResource;
            _resource.ResourceChanged += ResourceChanged;
            _resource.RemovedResource += RemovedResource;
            
            _image.sprite = _resource.Sprite;
            _shadow.effectColor = _resource.ShadowColor;
            _text.text = string.Format(_textFormat, _resource.Value);
            _currentValue = _resource.Value;
        }

        private void OnDisable()
        {
            _resource.AddedResource -= AddedResource;
            _resource.RemovedResource -= RemovedResource;
        }

        private void Update()
        {
            if (_resource.Value != (int)_currentValue)
            {
                float diff = _resource.Value - _currentValue;
                
                _currentValue += (Time.deltaTime * _incrementXSecond) * Mathf.Sign(diff);
                
                _text.text = string.Format(_textFormat, (int)_currentValue);
            }
        }

        private void ResourceChanged(int newValue)
        {
            
        }

        private void RemovedResource(int delta)
        {
        }

        private void AddedResource(int delta)
        {
        }
    }
}