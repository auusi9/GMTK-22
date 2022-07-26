﻿using System;
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
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private string _textFormat = "{0}";
        [SerializeField] private float _incrementXSecond = 5f;

        private float _currentValue = 0;
        
        private void OnEnable()
        {
            _resource.AddedResource += AddedResource;
            _resource.ResourceChanged += ResourceChanged;
            _resource.RemovedResource += RemovedResource;
            
            _image.sprite = _resource.Sprite;
            Color shadowColor = _resource.ShadowColor;
            shadowColor.a = 0.75f;
            _shadow.effectColor = shadowColor;
            _text.text = string.Format(_textFormat, _resource.Value);
            _currentValue = _resource.Value;
            _description.text = _resource.ResourceName;
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

                float inc = _incrementXSecond;
                if (_incrementXSecond < Mathf.Abs(diff))
                {
                    inc += (Mathf.Abs(diff) - _incrementXSecond);
                }

                _currentValue += (Time.deltaTime * inc) * Mathf.Sign(diff);
                
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