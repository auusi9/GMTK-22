using System;
using CityBuilder;
using MainMenu;
using UnityEngine;

namespace Dice
{
    public class GrantFace : MonoBehaviour
    {
        [SerializeField] private Face _face;
        [SerializeField] private DiceInventory _diceInventory;

        public event Action<Face> NewFace;
        
        public void Grant(Building building, SaveFace saveFace)
        {
            if (saveFace != null && saveFace.HasFace)
            {
                Face newface = Instantiate(_face);
                building.NewFace(newface);
                _diceInventory.Dice[saveFace.DiceId].AddFace(newface, saveFace.Position);
                return;
            }
            else if(saveFace != null && !saveFace.HasFace)
            {
                return;
            }
            
            
            _diceInventory.NewFaceAccepted += NewFaceAccepted;
            _diceInventory.NewFaceDiscarded += NewFaceDiscarded;
            
            Face face = Instantiate(_face);
            
            building.NewFace(face);
            _diceInventory.NewFace(face);
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