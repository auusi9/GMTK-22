using System.Collections.Generic;
using System.Linq;
using Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    public class RollDicesMenu : MonoBehaviour
    {
        [SerializeField] private RollDice _rollDicePrefab;
        [SerializeField] private Button _rollButton;
        [SerializeField] private Toggle _selectAll;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Transform _parent;
        [SerializeField] private DiceInventory _diceInventory;
        [SerializeField] private Resource _gold;

        private List<RollDice> _dicesSelected = new List<RollDice>();
        private List<RollDice> _allDices = new List<RollDice>();

        private void Start()
        {
            foreach (var die in _diceInventory.Dice)
            {
                NewDice(die);
            }

            _diceInventory.NewDice += NewDice;
            _gold.ResourceChanged += GoldChanged;
        }

        private void OnDestroy()
        {
            _diceInventory.NewDice -= NewDice;
            _gold.ResourceChanged -= GoldChanged;
        }

        private void NewDice(Die die)
        {
            RollDice rollDice = Instantiate(_rollDicePrefab, _parent);
            rollDice.Die.SetDie(die);
            rollDice.Die.DieSelected += DieSelected;
            _allDices.Add(rollDice);
        }

        private void DieSelected(DieUI selectedDie)
        {
            RollDice selectedDice = _dicesSelected.FirstOrDefault(x => x.Die == selectedDie);
            if (selectedDice)
            {
                _dicesSelected.Remove(selectedDice);
                selectedDie.SetDeselected();
                CalculatePrice();
                
                if (_dicesSelected.Count != _allDices.Count)
                {
                    _selectAll.SetIsOnWithoutNotify(false);
                }
                return;
            }
            
            selectedDice = _allDices.FirstOrDefault(x => x.Die == selectedDie);
            _dicesSelected.Add(selectedDice);
            selectedDie.SetSelected();
            CalculatePrice();

            if (_dicesSelected.Count == _allDices.Count)
            {
                _selectAll.SetIsOnWithoutNotify(true);
            }
        }

        private void CalculatePrice()
        {
            var price = GetTotalPrice();

            _priceText.text = price.ToString();

            if (price > _gold.Value || _dicesSelected.Count == 0)
            {
                _rollButton.interactable = false;
            }
            else
            {
                _rollButton.interactable = true;
            }
        }

        private int GetTotalPrice()
        {
            int price = 0;
            foreach (var dice in _dicesSelected)
            {
                price = dice.Die.Die.GetPrice();
            }

            return price;
        }

        private void GoldChanged(int obj)
        {
            CalculatePrice();
        }

        public void RollSelectedDice()
        {
            foreach (var die in _dicesSelected)
            {
                die.Roll();
            }
            
            _gold.RemoveResource(GetTotalPrice());
        }

        public void SelectAll(bool select)
        {
            if (_allDices.Count == _dicesSelected.Count)
            {
                _dicesSelected.Clear();
                foreach (var die in _allDices)
                {
                    die.Die.SetDeselected();
                }

                CalculatePrice();
                return;
            }
            
            foreach (var die in _allDices)
            {
                die.Die.SetSelected();
                if (!_dicesSelected.Contains(die))
                {
                    _dicesSelected.Add(die);
                }
            }
            CalculatePrice();
        }
    }
}