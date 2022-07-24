using System;
using System.Collections.Generic;
using Resources;
using UnityEngine;

namespace Dice
{
    [CreateAssetMenu(menuName = "Dice/Dice Inventory", fileName = "Dice Inventory", order = 0)]
    public class DiceInventory : ScriptableObject
    {
        [SerializeField] private Resource _diceAmount;
        [SerializeField] private Die _die;

        private List<Die> _dice = new List<Die>();

        public List<Die> Dice => _dice;

        public Action<Face> NewFaceAdded;
        public Action NewFaceDiscarded;
        public Action<Face> NewFaceAccepted;
        public Action<Die> NewDice;

        private void OnEnable()
        {
            _diceAmount.ResourceChanged += ResourceChanged;
            ResourceChanged(_diceAmount.Value);
        }

        private void OnDisable()
        {
            _diceAmount.ResourceChanged -= ResourceChanged;
            
            foreach (var dice in _dice)
            {
                DestroyImmediate(dice);
            }
        }

        private void ResourceChanged(int obj)
        {
            if (_dice.Count < obj)
            {
                _dice.Add(ScriptableObject.Instantiate(_die));
            }
        }

        public void AddNewDice()
        {
            Die die = ScriptableObject.Instantiate(_die);
            _dice.Add(die);
            NewDice?.Invoke(die);
        }
        
        public void NewFace(Face face)
        {
            NewFaceAdded?.Invoke(face);
        }

        public void DiscardedNewFace()
        {
            NewFaceDiscarded?.Invoke();
        }
        
        public void AcceptedNewFace(Face face)
        {
            NewFaceAccepted?.Invoke(face);
        }
    }
}