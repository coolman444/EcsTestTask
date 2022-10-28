using UnityEngine;

namespace Script
{
    public class TestCharacterController : MonoBehaviour
    {
        private InputActions _actions;
            
        [SerializeField]
        private Camera _camera;
        
        [SerializeField]
        private LayerMask _groundLayers;

        [SerializeField] 
        private MoveTowards _moveTowards;

        private void Awake()
        {
            _actions = new InputActions();
            _actions.Gameplay.MoveCommand.performed += _ =>
            {
                TryMove();
            };
            
            if (!_camera)
            {
                _camera = Camera.main;
            }
        }

        private void OnValidate()
        {
            if (!_moveTowards)
            {
                _moveTowards = GetComponent<MoveTowards>();
            }
        }

        private void OnEnable()
        {
            _actions.Enable();
        }

        private void OnDisable()
        {
            _actions.Disable();
        }

        private void TryMove()
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(_actions.Gameplay.MovePoint.ReadValue<Vector2>()), out var hit, 1000, _groundLayers))
            {
                _moveTowards.Destination = hit.point;
            }
        }
    }
}
