using System;
using System.Collections;
using UnityEngine;

namespace Dice
{
    public class RollDice : MonoBehaviour
    {
        [SerializeField] private DieUI _dieUI;
        [SerializeField] private float _faceChangeTime = 0.15f;
        [SerializeField] private float _rollTime = 1f;

        public DieUI Die => _dieUI;
        
        public event Action<Face> DieRolled;

        public void Roll()
        {
            _dieUI.ChangeFace();
            StartCoroutine(RollRoutine());
        }

        private IEnumerator RollRoutine()
        {
            float time = 0f;
            float faceChange = 0f;
            while (time <= _rollTime)
            {
                if (faceChange >= _faceChangeTime)
                {
                    _dieUI.ChangeFace();
                }

                yield return 0;
                time += Time.deltaTime;
                faceChange += Time.deltaTime;
            }

            Face face = _dieUI.Die.GetRandomFace();
            _dieUI.SetFace(face);
            DieRolled?.Invoke(face);
        }
    }
}