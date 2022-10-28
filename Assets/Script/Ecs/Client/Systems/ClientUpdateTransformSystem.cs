using Leopotam.EcsLite;
using Script.Ecs.Client.Components;
using Script.Ecs.Components;
using UnityEngine;

namespace Script.Ecs.Client.Systems
{
    public class ClientUpdateTransformSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var positionPool = world.GetPool<Position>();
            var rotationPool = world.GetPool<Rotation>();
            var gameObjectPool = world.GetPool<ClientGameObjectRef>();
            
            var positionFilter = world
                .Filter<ClientUpdateTransform>()
                .Inc<ClientGameObjectRef>()
                .Inc<Position>()
                .End();
            foreach (var entity in positionFilter)
            {
                var transform = gameObjectPool.Get(entity).Ref.transform;
                transform.position = positionPool.Get(entity).Value;
            }
            
            var rotationFilter = world
                .Filter<ClientUpdateTransform>()
                .Inc<ClientGameObjectRef>()
                .Inc<Rotation>()
                .End();
            foreach (var entity in rotationFilter)
            {
                var transform = gameObjectPool.Get(entity).Ref.transform;
                transform.rotation = Quaternion.Euler(0, rotationPool.Get(entity).Value, 0);
            }
        }
    }
}