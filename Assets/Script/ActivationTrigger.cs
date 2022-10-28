using UnityEngine;
using UnityEngine.Events;

namespace Script
{
    public class ActivationTrigger : MonoBehaviour
    {
        private int _count;
        
        public UnityEvent OnActivate;
        public UnityEvent OnDeactivate;
        
        private void OnTriggerEnter(Collider other)
        {
            if (++_count == 1)
            {
                Debug.Log("Trigger activated");
                OnActivate?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (--_count == 0)
            {
                Debug.Log("Trigger deactivated");
                OnDeactivate?.Invoke();
            }
        }
    }
}
