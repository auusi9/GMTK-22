using UnityEngine;

namespace Workers
{
    public class WorkerSpot : MonoBehaviour
    {
        private Worker _currentWorker;
        public bool Available => !_currentWorker;

        public Worker Worker => _currentWorker;
        
        public void SetWorker(Worker worker)
        {
            _currentWorker = worker;
            worker.transform.SetParent(transform); 
            _currentWorker.transform.localPosition = Vector3.zero;

            if (_currentWorker.CurrentSpot != null && _currentWorker.CurrentSpot != this)
            {
                _currentWorker.CurrentSpot.EmptySpot();
            }
            
            _currentWorker.CurrentSpot = this;
        }

        private void EmptySpot()
        {
            _currentWorker = null;
        }
    }
}