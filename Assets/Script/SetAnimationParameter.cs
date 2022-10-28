using UnityEngine;

namespace TutorialInfo.Scripts
{
    public class SetAnimationParameter : MonoBehaviour
    {
        private int _parameterHash;
        
        [SerializeField]
        private string _parameterName;

        [SerializeField]
        private Animator _animator;

        private void Awake()
        {
            _parameterHash = Animator.StringToHash(_parameterName);
        }

        private void OnValidate()
        {
            if (!_animator)
            {
                _animator = GetComponent<Animator>();
            }
        }

        public bool BoolValue
        {
            get => _animator.GetBool(_parameterHash);
            set => _animator.SetBool(_parameterHash, value);
        }
    }
}