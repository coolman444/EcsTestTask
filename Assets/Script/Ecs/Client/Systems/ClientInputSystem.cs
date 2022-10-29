using Leopotam.EcsLite;
using Script.Ecs.Client.Components;
using Script.Ecs.Components;
using UnityEngine;

namespace Script.Ecs.Client.Systems
{
    public class ClientInputSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly int _groundLayers;
        
        public ClientInputSystem(ClientSettings settings)
        {
            _groundLayers = settings.GroundLayers;
        }

        private readonly InputActions _actions = new ();
        private Camera _camera;
        
        public void Init(IEcsSystems systems)
        {
            _camera = Camera.main;
            _actions.Enable();
        }
        
        public void Run(IEcsSystems systems)
        {
            if (_actions.Gameplay.MoveCommand.WasPerformedThisFrame())
            {
                if (Physics.Raycast(_camera.ScreenPointToRay(_actions.Gameplay.MovePoint.ReadValue<Vector2>()), out var hit, 1000, _groundLayers))
                {
                    var world = systems.GetWorld();
                    var destinationPointPool = world.GetPool<DestinationPoint>();
                    foreach (var entity in world.Filter<ClientTakeInput>().End())
                    {
                        destinationPointPool.GetOrAdd(entity).Value = hit.point;
                    }
                }
            }
        }
    }
}