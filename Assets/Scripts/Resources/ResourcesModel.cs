using System.Collections.Generic;
using UnityEngine;

namespace Resources
{
    [CreateAssetMenu(menuName = "Resources/ResourceModel", fileName = "ResourceModel", order = 0)]
    public class ResourcesModel : ScriptableObject
    {
        [SerializeField] private List<Resource> _resources;
    }
}