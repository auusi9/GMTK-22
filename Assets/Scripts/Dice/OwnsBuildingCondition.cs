using CityBuilder;
using UnityEngine;

namespace Dice
{
    [CreateAssetMenu(menuName = "Dice/Combo Condition/Owns Building", fileName = "Owns{Name}Condition", order = 1)]
    public class OwnsBuildingCondition : FaceComboCondition
    {
        [SerializeField] private Building _buildingNeeded;
        [SerializeField] private BuildingLibrary _buildingLibrary;
        
        public override bool IsValid()
        {
            return _buildingLibrary.HasThisBuilding(_buildingNeeded);
        }
    }
}