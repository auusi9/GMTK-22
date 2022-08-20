using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Dice.FaceUIBehaviours
{
    public class ColorBlink : UIBehaviour
    {
        [SerializeField] private List<ImageToChange> _maskableGraphic;
        [SerializeField] private float _blinkTime = 0.1f;

        private Coroutine _animation;
        
        [ContextMenu("DoBehaviour")]
        public override void DoBehaviour()
        {
            if(_animation != null)
                StopCoroutine(_animation);
            
            _animation = StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            foreach (var graphic in _maskableGraphic)
            {
                graphic.Graphic.DOColor(graphic.ChangeValue, _blinkTime / 2f).SetUpdate(true);
            }
            
            yield return new WaitForSecondsRealtime(_blinkTime / 2f);
            
            foreach (var graphic in _maskableGraphic)
            {
                graphic.Graphic.DOColor(graphic.DefaultValue, _blinkTime / 2f).SetUpdate(true);
            }
            
            _animation = null;
        }
        
        [Serializable]
        public class ImageToChange
        {
            public Color ChangeValue;
            public Color DefaultValue;
            public MaskableGraphic Graphic;
        }
    }
}