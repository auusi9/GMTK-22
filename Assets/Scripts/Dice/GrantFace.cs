using System;
using UnityEngine;

namespace Dice
{
    public class GrantFace : MonoBehaviour
    {
        [SerializeField] private Face _face;
        [SerializeField] private DiceInventory _diceInventory;

        public Action<Face> NewFace;
        
        public void Grant()
        {   
            _diceInventory.NewFaceAccepted += NewFaceAccepted;
            _diceInventory.NewFaceDiscarded += NewFaceDiscarded;
            _diceInventory.NewFace(_face);
        }

        private void NewFaceAccepted(Face newFace)
        {
            _diceInventory.NewFaceAccepted -= NewFaceAccepted;
            _diceInventory.NewFaceDiscarded -= NewFaceDiscarded;
            
            NewFace?.Invoke(newFace);
        }

        private void NewFaceDiscarded()
        {
            _diceInventory.NewFaceAccepted -= NewFaceAccepted;
            _diceInventory.NewFaceDiscarded -= NewFaceDiscarded;
        }
    }
}