using UnityEngine;
using UnityEngine.UI;

public class ShaderUnscaledTime : MonoBehaviour
{
    [SerializeField] private Image _image;
    private static readonly int UnscaledTime = Shader.PropertyToID("_UnscaledTime");

    // Update is called once per frame
    void Update()
    {
        _image.material.SetFloat(UnscaledTime, Time.unscaledTime);
    }

    private void OnDisable()
    {
        _image.material.SetFloat(UnscaledTime, 0f);
    }
}
