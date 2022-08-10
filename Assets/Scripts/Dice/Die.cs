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
        [SerializeField] private Face _defaultFace;

        public event Action NewFace;
        
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

            _faces[index] = face;
            NewFace?.Invoke();
            return _faces[index];
        }

        public Face GetFirstFace()
        {
            if(_faces == null)
                OnEnable();
            
            for (var i = 0; i < _faces.Length; i++)
            {
                if (_faces[i] != null)
                {
                    return _faces[i];
                }
            }

            return _defaultFace;
        }

        public Face GetDefaultFace()
        {
            return _defaultFace;
        }

        public Face GetRandomFace(Face currentFace)
        {
            if(_faces == null)
                OnEnable();

            List<Face> possibleFaces = new List<Face>();

            bool defaultIconAdded = false;
            for (var i = 0; i < _faces.Length; i++)
            {
                if (_faces[i] != null)
                {
                    if (_faces[i].Sprite != currentFace.Sprite && _faces[i].CurrentReward != currentFace.CurrentReward)
                    {
                        possibleFaces.Add(_faces[i]);
                    }
                }
                else if(_defaultFace.Sprite != currentFace.Sprite && defaultIconAdded == false)
                {
                    defaultIconAdded = true;
                    possibleFaces.Add(_defaultFace);
                }
            }

            if (possibleFaces.Count > 0)
            {
                int randomNumber = Random.Range(0, possibleFaces.Count);
                return possibleFaces[randomNumber];
            }

            return _defaultFace;
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