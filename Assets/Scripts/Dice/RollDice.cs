using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Resources;

namespace Dice
{
    public class RollDice : MonoBehaviour
    {
        [SerializeField] private DieUI _dieUI;
        [SerializeField] private ShowAmountEarned _showAmountEarned;
        [SerializeField] private ShowAmountEarned _showAmountEarnedCombo;
        [SerializeField] private float _faceChangeTime = 0.15f;
        [SerializeField] private float _rollTime = 1f;
        [SerializeField] private RectTransform _containerRectTrans;
        [SerializeField] private float _strength = 10f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _randomness = 30f;

        public DieUI Die => _dieUI;
        
        public event Action<RollDice, Face> DieRolled;

        public void Roll()
        {
            _dieUI.ChangeFace();
            StartCoroutine(RollRoutine());
            _containerRectTrans.DOShakeRotation(_rollTime, new Vector3(0,0, _strength), _vibrato, _randomness, true);
            _containerRectTrans.DOShakePosition(_rollTime, new Vector3(_strength, _strength, _strength), _vibrato, _randomness, true);
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
                    faceChange = 0f;
                }

                yield return 0;
                time += Time.deltaTime;
                faceChange += Time.deltaTime;
            }

            Face face = _dieUI.Die.GetRandomFace();
            _dieUI.SetFace(face);
            DieRolled?.Invoke(this, face);
        }

        public void ShowAmountEarned(int amount, Resource resource)
        {
            if(amount == 0)
                return;
            
            _showAmountEarned.Show(amount, resource);
        }        
        
        public void ShowAmountEarnedCombo(int amount, Resource resource)
        {
            if(amount == 0)
                return;
            
            _showAmountEarnedCombo.Show(amount, resource);
        }
    }
}