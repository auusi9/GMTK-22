using System.Collections.Generic;
using System.Linq;
using Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UIGeneric;

namespace Dice
{
    public class RollDicesMenu : MonoBehaviour
    {
        [SerializeField] private RollDice _rollDicePrefab;
        [SerializeField] private Toggle _selectAll;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Transform _parent;
        [SerializeField] private DiceInventory _diceInventory;
        [SerializeField] private Resource _gold;
        [SerializeField] private Image _rollerBlocker;
        [SerializeField] private ButtonAnimations _newRollButton;

        private List<RollDice> _dicesSelected = new List<RollDice>();
        private List<RollDice> _allDices = new List<RollDice>();
        private List<Face> _rolledFaces = new List<Face>();

        private void Start()
        {
            foreach (var die in _diceInventory.Dice)
            {
                NewDice(die);
            }

            _diceInventory.NewDice += NewDice;
            _gold.ResourceChanged += GoldChanged;

            _newRollButton.SetEnable(_dicesSelected.Count != 0);
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
            rollDice.DieRolled += DieRolled;
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
                _newRollButton.SetEnable(false);
            }
            else
            {
                _newRollButton.SetEnable(true);
            }
        }

        private int GetTotalPrice()
        {
            int price = 0;
            foreach (var dice in _dicesSelected)
            {
                price += dice.Die.Die.GetPrice();
            }

            return price;
        }

        private void GoldChanged(int obj)
        {
            CalculatePrice();
        }

        public void RollSelectedDice()
        {
            _rollerBlocker.gameObject.SetActive(true);
            _newRollButton.SetEnable(false);
            _selectAll.interactable = false;
            _rolledFaces.Clear();
            
            foreach (var die in _dicesSelected)
            {
                die.Roll();
            }
            
            _gold.RemoveResource(GetTotalPrice());
        }
        
        private void DieRolled(Face face)
        {
            _rolledFaces.Add(face);
            
            if (_dicesSelected.Count == _rolledFaces.Count)
            {
                FinishedRolling();
            }
        }

        private void FinishedRolling()
        {
            IEnumerable<IGrouping<Resource, Face>> groupBy = _rolledFaces.Where(x => x != null).GroupBy(x => x.Resource);

            foreach (var dieFace in groupBy)
            {
                int number = 0;
                Face lastFace = null;
                foreach (var face in dieFace)
                {
                    face.GiveReward();
                    lastFace = face;
                    number++;
                }

                if (lastFace != null) lastFace.GiveCombo(number);
            }
            
            _rolledFaces.Clear();
            _rollerBlocker.gameObject.SetActive(false);
            _selectAll.interactable = true;
            CalculatePrice();
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