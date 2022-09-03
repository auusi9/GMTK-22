using System;
using System.Collections.Generic;
using Dice;
using Resources;
using UnityEngine;
using Workers;

namespace CityBuilder
{
    public class Scenery : MonoBehaviour
    {
        [SerializeField] private Resource _resource;
        [SerializeField] private int _defaultSceneryAmount = 15;
        [SerializeField] private int _defaultImprovedSceneryAmount = 30;
        [SerializeField] private int _sceneriesToImprove = 4;
        [SerializeField] private DayNight _dayNight;
        [SerializeField] private Building _building;

        private int _currentValue = 0;

        public event Action<Face> SceneryAmountChanged;
        public event Action SceneryReset;

        private List<Scenery> _sceneriesFriends = new List<Scenery>();
        private bool _isImproved = false;

        public List<Scenery> SceneriesFriends => _sceneriesFriends;

        private void OnEnable()
        {
            _currentValue = _defaultSceneryAmount;
            _dayNight.StartDay += OnNewDay;
            _building.NewNeighbour += NewNeighbour;
        }

        private void OnDestroy()
        {
            _building.NewNeighbour -= NewNeighbour;
            _dayNight.StartDay -= OnNewDay;
        }

        private void NewNeighbour()
        {
            _sceneriesFriends.Clear();
            _sceneriesFriends.Add(this);
            GetSceneries(_building);

            foreach (var sceneries in _sceneriesFriends)
            {
                if(sceneries != this)
                    sceneries.SetSceneriesFriends(_sceneriesFriends);
            }

            SetImproved();
        }

        private void GetSceneries(Building building)
        {
            Building[] buildings = building.Get4NearBuildings();
            foreach (var b in buildings)
            {
                if (b != null && b.Scenery != null && b.Scenery.Resource == _resource)
                {
                    if (!_sceneriesFriends.Contains(b.Scenery))
                    {
                        _sceneriesFriends.Add(b.Scenery);
                        GetSceneries(b);
                    }
                }
            }
        }

        private void SetImproved()
        {
            if (_isImproved && _sceneriesFriends.Count >= _sceneriesToImprove)
            {
                return;
            }

            if (_isImproved)
            {
                _currentValue -= _defaultImprovedSceneryAmount - _defaultSceneryAmount;
                SceneryAmountChanged?.Invoke(null);
                _isImproved = false;
                return;
            }

            if (_sceneriesFriends.Count >= _sceneriesToImprove)
            {
                _currentValue += _defaultImprovedSceneryAmount - _defaultSceneryAmount;
                _isImproved = true;
                SceneryAmountChanged?.Invoke(null);
            }
        }

        public void SetSceneriesFriends(List<Scenery> sceneries)
        {
            _sceneriesFriends = sceneries;

            SetImproved();
        }

        private void OnNewDay()
        {
            if (_sceneriesFriends.Count >= _sceneriesToImprove)
            {
                _currentValue = _defaultImprovedSceneryAmount;
                return;
            }
            
            _currentValue = _defaultSceneryAmount;
        }

        public Resource Resource => _resource;
        public int CurrentValue => _currentValue;
        public float Percentage
        {
            get
            {
                if (_sceneriesFriends.Count >= _sceneriesToImprove)
                {
                    return _currentValue / (float)_defaultImprovedSceneryAmount;
                }
            
                return _currentValue / (float)_defaultSceneryAmount;
            }
        }

        public void SpendScenery(Face face)
        {
            _currentValue--;
            SceneryAmountChanged?.Invoke(face);
        }

        public void ResetScenery()
        {
            _currentValue = _defaultSceneryAmount;
            SceneryReset?.Invoke();
        }
    }
}