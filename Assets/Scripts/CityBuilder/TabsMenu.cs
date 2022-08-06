using Dice;
using UnityEngine;
using UnityEngine.UI;

namespace CityBuilder
{
    public class TabsMenu : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _craftButton;
        [SerializeField] private Button _diceButton;
        [SerializeField] private GameObject _craftMenu;
        [SerializeField] private GameObject _diceMenu;
        [SerializeField] private Sprite _buttonEnabled;
        [SerializeField] private Sprite _buttonDisabled;


        public void CloseButtonPressed()
        {
            _craftMenu.SetActive(false);
            _diceMenu.SetActive(false);
            _closeButton.image.sprite = _buttonEnabled;
            _craftButton.image.sprite = _buttonDisabled;
            _diceButton.image.sprite = _buttonDisabled;
        }
        
        public void CraftButtonPressed()
        {
            if (_craftMenu.activeSelf)
            {
                _craftMenu.SetActive(false);
                _craftButton.image.sprite = _buttonDisabled;
                
                if(!_diceMenu.activeSelf)
                    _closeButton.image.sprite = _buttonEnabled;
                
                return;
            }
            
            _craftMenu.SetActive(true);
            _closeButton.image.sprite = _buttonDisabled;
            _craftButton.image.sprite = _buttonEnabled;
        }
        
        public void DiceButtonPressed()
        {
            if (_diceMenu.activeSelf)
            {
                _diceMenu.SetActive(false);
                _diceButton.image.sprite = _buttonDisabled;
                
                if(!_craftMenu.activeSelf)
                    _closeButton.image.sprite = _buttonEnabled;

                return;
            }
            
            _diceMenu.SetActive(true);
            _closeButton.image.sprite = _buttonDisabled;
            _diceButton.image.sprite = _buttonEnabled;
        }
    }
}