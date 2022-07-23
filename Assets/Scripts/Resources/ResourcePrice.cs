using CityBuilder;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Resources
{
    public class ResourcePrice : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private string _textFormat = "{0}";
        [SerializeField] private float _incrementXSecond = 5f;

        
        public void SetBuildingCost(BuildingCost cost)
        {
            gameObject.SetActive(true);
            _image.sprite = cost.Resource.Sprite;
            _text.text = string.Format(_textFormat, cost.Cost);
        }
    }
}