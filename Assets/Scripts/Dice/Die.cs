using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Dice
{
    [CreateAssetMenu(menuName = "Dice/Die", fileName = "Die", order = 2)]
    public class Die : ScriptableObject
    {
        [SerializeField] private Sprite _defaultIcon;
        [SerializeField] private Color _defaultColor;
        
        private Face[] _faces = new Face[6];
        
        public Face[] Faces => _faces;

        private void OnEnable()
        {
            _faces = new Face[6];
        }

        private void OnDestroy()
        {
            if(_faces == null)
                return;
                
            for (var i = 0; i < _faces.Length; i++)
            {
                if (_faces[i] != null)
                {
                    DestroyImmediate(_faces[i]);
                }
            }
        }

        public Face AddFace(Face face, int index)
        {
            if (_faces[index] != null)
            {
                _faces[index].ToDestroy();
            }

            _faces[index] = Object.Instantiate(face);
            return _faces[index];
        }

        public Sprite GetFirstSprite(out Color shadowColor)
        {
            shadowColor = _defaultColor;
            if(_faces == null)
                OnEnable();
            
            for (var i = 0; i < _faces.Length; i++)
            {
                if (_faces[i] != null)
                {
                    shadowColor = _faces[i].ShadowColor;
                    return _faces[i].Sprite;
                }
            }

            return _defaultIcon;
        }

        public Sprite GetDefaultSprite(out Color shadowColor)
        {
            shadowColor = _defaultColor;
            return _defaultIcon;
        }

        public Sprite GetRandomSprite(Sprite iconSprite, out Color shadowColor)
        {
            if(_faces == null)
                OnEnable();

            List<Sprite> possibleSprites = new List<Sprite>();
            List<Color> possibleColor = new List<Color>();

            bool defaultIconAdded = false;
            for (var i = 0; i < _faces.Length; i++)
            {
                if (_faces[i] != null)
                {
                    if (_faces[i].Sprite != iconSprite)
                    {
                        possibleSprites.Add(_faces[i].Sprite);
                        possibleColor.Add(_faces[i].ShadowColor);
                        
                    }
                }
                else if(_defaultIcon != iconSprite && defaultIconAdded == false)
                {
                    defaultIconAdded = true;
                    possibleSprites.Add(_defaultIcon);
                    possibleColor.Add(_defaultColor);
                }
            }

            if (possibleSprites.Count > 0)
            {
                int randomNumber = Random.Range(0, possibleSprites.Count);
                shadowColor = possibleColor[randomNumber];
                return possibleSprites[randomNumber];
            }

            shadowColor = _defaultColor;
            return _defaultIcon;
        }

        public int GetPrice()
        {
            if(_faces == null)
                OnEnable();

            int price = 0;
            for (var i = 0; i < _faces.Length; i++)
            {
                if (_faces[i] != null)
                {
                    price += _faces[i].GetPrice();
                }
            }

            return price;
        }
        
        public Face GetRandomFace()
        {
            if(_faces == null)
                OnEnable();

            return _faces[Random.Range(0, _faces.Length)];
        }
    }
}