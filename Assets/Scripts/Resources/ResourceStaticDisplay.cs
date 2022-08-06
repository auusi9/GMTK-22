using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Resources
{
    public class ResourceStaticDisplay : MonoBehaviour
    {
        [SerializeField] private Resource _resource;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private string _textFormat = "{0}";

        public void SetInformation(Resource resource, int amount)
        {
            _resource = resource;
            _image.sprite = _resource.Sprite;
            _text.text = string.Format(_textFormat, amount);
        }
    }
}