using System;
using System.Collections.Generic;
using System.Linq;
using Resources;
using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    [CreateAssetMenu(menuName = "Dice/Dice Inventory", fileName = "Dice Inventory", order = 0)]
    public class DiceInventory : ScriptableObject
    {
        [SerializeField] private Resource _diceAmount;
        [SerializeField] private Die _die;

        private List<Die> _dice = new List<Die>();

        public List<Die> Dice => _dice;

        public event Action<Face> NewFaceAdded;
        public event Action NewFaceDiscarded;
        public event Action<Face> NewFaceAccepted;
        public event Action<Die> NewDice;

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
            
            _dice.Clear();
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

        public int GetFaceIndex(Face face, out int index)
        {
            index = 0;
            for (var i = 0; i < _dice.Count; i++)
            {
                var dice = _dice[i];
                for (var j = 0; j < dice.Faces.Length; j++)
                {
                    var diceFace = dice.Faces[j];
                    index = j;
                    if (Equals(diceFace, face))
                    {  
                        return i;
                    }
                }
            }

            return 0;
        }
    }
}