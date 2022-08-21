using System;
using System.Collections.Generic;
using System.Linq;
using CityBuilder;
using Dice;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Resources;
using UnityEngine;

namespace MainMenu
{
    [CreateAssetMenu(menuName = "SaveManager/SaveManager", fileName = "SaveManager", order = 0)]
    public class SaveManager : ScriptableObject
    {
        [SerializeField] private CityMapLocator _cityMapLocator;
        [SerializeField] private DiceInventory _diceInventory;
        [SerializeField] private List<Resource> _resources;

        private const string SAVE_DATA_KEY = "save_data";
        
        private SaveData _saveData;
        public bool HasSaveData => _saveData != null;

        private void OnEnable()
        {
            string save = PlayerPrefs.GetString(SAVE_DATA_KEY, "");
            
            if (!string.IsNullOrEmpty(save))
            {
                _saveData = JsonConvert.DeserializeObject<SaveData>(save);
            }
        }
        
        public void LoadData()
        {
            if (_saveData != null && _saveData.IsValid())
            {
                for (int i = 1; i < _saveData.DiceAmount; i++)
                {
                    _diceInventory.AddNewDice();
                }

                foreach (var res in _saveData.Resources)
                {
                    var resource = _resources.FirstOrDefault(x => res.ResourceId == x.ResourceName);

                    if (resource != null)
                    {
                        resource.SetDefaultValue(res.CurrentValue);
                    }
                }


                _cityMapLocator.SetSavedBuildings(_saveData.Buildings);
            }
        }

        public void SaveData()
        {
            _saveData = new SaveData();
            var buildings = _cityMapLocator.GetBuildingsToSave();

            if (buildings == null)
                return;
            _saveData.Buildings = buildings;
            _saveData.DiceAmount = _diceInventory.Dice.Count;
            _saveData.Resources = GetResourcesData();

            if (_saveData != null)
            {
                string save = JsonConvert.SerializeObject(_saveData);

                PlayerPrefs.SetString(SAVE_DATA_KEY, save);
                PlayerPrefs.Save();
            }
        }

        private List<SaveResource> GetResourcesData()
        {
            List<SaveResource> resources = new List<SaveResource>(_resources.Count);

            foreach (var t in _resources)
            {
                resources.Add(new SaveResource()
                {
                    CurrentValue = t.Value,
                    ResourceId = t.ResourceName
                });
            }

            return resources;
        }
    }
    
    [Serializable]
    public class SaveData
    {
        [CanBeNull] [ItemCanBeNull] public SaveBuilding[] Buildings;
        [CanBeNull] [ItemCanBeNull] public List<SaveResource> Resources;
        public int? DiceAmount;

        public bool IsValid()
        {
            return Buildings is { Length: > 0 };
        }
    }

    [Serializable]
    public class SaveBuilding
    {
        [CanBeNull] public string BuildingId;
        [CanBeNull] [ItemCanBeNull] public List<SaveWorkerSpot> Spots = new List<SaveWorkerSpot>();
        [CanBeNull] public SaveFace SaveFace;
    }
    
    [Serializable]
    public class SaveFace
    {
        public int DiceId;
        public int Position;
        public bool HasFace;
    }
    
    [Serializable]
    public class SaveWorkerSpot
    {
        [CanBeNull] public SaveWorker Worker;
    }
    
    [Serializable]
    public class SaveWorker
    {
        public float CurrentEnergy;
    }
    
    [Serializable]
    public class SaveResource
    {
        [CanBeNull] public string ResourceId;
        public int CurrentValue;
    }
}