using System.Collections.Generic;
using System.Linq;
using Dice.FaceUIBehaviours;
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
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Transform _parent;
        [SerializeField] private DiceInventory _diceInventory;
        [SerializeField] private Resource _gold;
        [SerializeField] private Image _rollerBlocker;
        [SerializeField] private ButtonAnimations _newRollButton;
        [SerializeField] private FaceUIBehaviour _notEnoughMoney;
        [SerializeField] private FaceUIBehaviour _enoughMoney;

        private List<RollDice> _dicesSelected = new List<RollDice>();
        private List<RollDice> _allDices = new List<RollDice>();
        private List<RolledFace> _rolledFaces = new List<RolledFace>();

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
            CalculatePrice();
        }

        private void DieSelected(DieUI selectedDie)
        {
            RollDice selectedDice = _dicesSelected.FirstOrDefault(x => x.Die == selectedDie);
            if (selectedDice)
            {
                _dicesSelected.Remove(selectedDice);
                selectedDie.SetDeselected();
                CalculatePrice();
                
                return;
            }
            
            selectedDice = _allDices.FirstOrDefault(x => x.Die == selectedDie);
            _dicesSelected.Add(selectedDice);
            selectedDie.SetSelected();
            CalculatePrice();
        }

        private void CalculatePrice()
        {
            var price = GetTotalPrice();

            _priceText.text = price.ToString();

            bool notEnoughMoney = price > _gold.Value;
            
            if (notEnoughMoney)
            {
                _notEnoughMoney.DoBehaviour();
            }
            else
            {
                _enoughMoney.DoBehaviour();
            }

            if (notEnoughMoney || _dicesSelected.Count == 0)
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
            _rolledFaces.Clear();
            
            foreach (var die in _dicesSelected)
            {
                die.Roll();
            }
            
            _gold.RemoveResource(GetTotalPrice());
        }
        
        private void DieRolled(RollDice die, Face face)
        {
            _rolledFaces.Add(new RolledFace(die, face));
            
            if (_dicesSelected.Count == _rolledFaces.Count)
            {
                FinishedRolling();
            }
        }

        public class RolledFace
        {
            public RollDice Die;
            public Face Face;

            public RolledFace(RollDice die, Face face)
            {
                Die = die;
                Face = face;
            }
        }
        
        private void FinishedRolling()
        {
            IEnumerable<IGrouping<Resource, RolledFace>> groupBy = _rolledFaces.Where(x => x.Face != null).GroupBy(x => x.Face.Resource);

            foreach (var dieFace in groupBy)
            {
                List<RolledFace> rolledFaces = dieFace.ToList();

                if (rolledFaces[0].Face.HasCombo)
                {
                    rolledFaces.Sort((x, y) => x.Face.CurrentReward.CompareTo(y.Face.CurrentReward));
                    
                    List<List<RolledFace>> list = rolledFaces.Select((x, i) => new { Index = i, Value = x })
                        .GroupBy(x => x.Index / rolledFaces[0].Face.ComboNeeded)
                        .Select(x => x.Select(v => v.Value).ToList())
                        .ToList();

                    foreach (var chunk in list)
                    {
                        GiveRewards(chunk);
                    }
                }
                else
                {
                    foreach (var face in rolledFaces)
                    {
                        face.Die.ShowAmountEarned(face.Face.GiveReward(0), face.Face.Resource);
                    }
                }
            }
            
            _rolledFaces.Clear();
            _rollerBlocker.gameObject.SetActive(false);
            CalculatePrice();
        }

        private void GiveRewards(List<RolledFace> rolledFaces)
        {
            int minReward = rolledFaces.Min(x => x.Face.CurrentReward);

            if (rolledFaces.Count % rolledFaces[0].Face.ComboNeeded == 0)
            {
                rolledFaces[0].Die.ShowAmountEarnedCombo(rolledFaces[0].Face.GiveCombo(minReward), rolledFaces[0].Face.ComboResource);
            }
            else
            {
                minReward = 0;
            }
            
            foreach (var rolledFace in rolledFaces)
            {
                rolledFace.Die.ShowAmountEarned(rolledFace.Face.GiveReward(minReward), rolledFace.Face.Resource);
            }
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