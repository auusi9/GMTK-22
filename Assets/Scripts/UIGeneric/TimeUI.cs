using UnityEngine;

namespace UIGeneric
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] private TimeManager _timeManager;

        public void PauseGame()
        {
            _timeManager.PauseGame(GetHashCode());
        }

        public void ResumeGame()
        {
            _timeManager.SetDefaultSpeed();
            _timeManager.ResumeGame(GetHashCode());
        }

        public void SpeedGame()
        {
            _timeManager.NextSpeed();
        }
    }
}