using UnityEngine;

namespace Script
{
    public class MoveTowards : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");

        private Vector3 _destination;

        [SerializeField]
        private float _stopDistance = 0.1f;
        
        [SerializeField]
        private float _moveAngle = 10;
        
        [SerializeField]
        private float _linearSpeed = 2;
        
        [SerializeField]
        private float _angularSpeed = 100;
        
        [SerializeField]
        private Animator _animator;

        public Vector3 Destination
        {
            get => _destination;
            set
            {
                _destination = value;
                enabled = true;
            }
        }

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            if (Vector3.Distance(Destination, transform.position) < _stopDistance)
            {
                _animator.SetFloat(Speed, 0);
                enabled = false;
                return;
            }

            var rotateAngle = Vector3.SignedAngle(transform.forward, Destination - transform.position, Vector3.up);
            if (Mathf.Abs(rotateAngle) < _moveAngle)
            {
                _animator.SetFloat(Speed, 1, 0.2f, Time.deltaTime);
                transform.Translate(transform.forward * _linearSpeed * Time.deltaTime * _animator.GetFloat(Speed), Space.World);
            }
            else
            {
                _animator.SetFloat(Speed, 0);
            }

            if (!Mathf.Approximately(rotateAngle, 0))
            {
                var deltaAngle = Mathf.MoveTowardsAngle(0, rotateAngle, _angularSpeed * Time.deltaTime);
                transform.Rotate(Vector3.up, deltaAngle, Space.World);
            }
        }
        
        private void OnValidate()
        {
            if (!_animator)
            {
                _animator = GetComponentInChildren<Animator>();
            }
        }
    }
}