using UnityEngine;

namespace Dice
{
    public class GrantFace : MonoBehaviour
    {
        [SerializeField] private Face _face;
        [SerializeField] private DiceInventory _diceInventory;
        
        public void Grant()
        {
            _diceInventory.NewFace(_face);
        }
    }
}