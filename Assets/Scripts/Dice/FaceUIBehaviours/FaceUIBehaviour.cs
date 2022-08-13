using System;
using System.Collections.Generic;
using CityBuilder;
using UnityEngine;
using UnityEngine.UI;

namespace Dice.FaceUIBehaviours
{
    public class FaceUIBehaviour : UIBehaviour
    {
        [SerializeField] private string _stateForUI;
        [SerializeField] private ObjectToChange[] _gameObjectsToChange;
        [SerializeField] private ImageToChange[] _images;
        
        [ContextMenu("DoBehaviour")]
        public override void DoBehaviour()
        {
            foreach (var go in _gameObjectsToChange)
            {
                go.Go.SetActive(go.Value);
            }

            foreach (var go in _images)
            {
                go.Graphic.color = go.Value;
            }
        }
    }

    public abstract class UIBehaviour : MonoBehaviour
    {
        public abstract void DoBehaviour();
    }

    [Serializable]
    public class ObjectToChange
    {
        public bool Value;
        public GameObject Go;
    }
    
    [Serializable]
    public class ImageToChange
    {
        public Color Value;
        public MaskableGraphic Graphic;
    }
}