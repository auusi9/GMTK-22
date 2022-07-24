using Resources;
using UnityEngine;

namespace CityBuilder
{
    public class Scenery : MonoBehaviour
    {
        [SerializeField] private Resource _resource;

        public Resource Resource => _resource;
    }
}