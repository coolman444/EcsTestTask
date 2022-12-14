using Leopotam.EcsLite;
using Script.Ecs.Components;
using UnityEngine;

namespace Script.Ecs.Systems
{
    public class ActiveDoorSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var positionPool = world.GetPool<Position>();
            var doorPool = world.GetPool<Script.Ecs.Components.Door>();
            var activePool = world.GetPool<Active>();
            var destinationPointPool = world.GetPool<DestinationPoint>();
            var originalPositionPool = world.GetPool<OriginalPosition>();
            var rotationPool = world.GetPool<Rotation>();
            var velocityPool = world.GetPool<Velocity>();
            
            var activeFilter = world
                .Filter<Active>()
                .Inc<OriginalPosition>()
                .Inc<Position>()
                .Inc<Rotation>()
                .Inc<Script.Ecs.Components.Door>()
                .End();
            
            foreach (var entity in activeFilter)
            {
                var door = doorPool.Get(entity);
                var forward = (Quaternion.Euler(0, rotationPool.Get(entity).Value, 0) * Vector3.forward);
                var targetPosition = 
                    originalPositionPool.Get(entity).Value + forward * doorPool.Get(entity).OpenDistance;  
                if (Vector3.Distance(positionPool.Get(entity).Value, targetPosition) < 0.01f)
                {
                    activePool.Del(entity);
                    continue;
                }
                
                velocityPool.GetOrAdd(entity).Value = forward * door.Speed;
                destinationPointPool.GetOrAdd(entity).Value = targetPosition;
            }
            
            var noActiveFilter = world
                .Filter<Position>()
                .Inc<OriginalPosition>()
                .Inc<Script.Ecs.Components.Door>()
                .Exc<Active>()
                .End();
            
            foreach (var entity in noActiveFilter)
            {
                destinationPointPool.Del(entity);
                velocityPool.Del(entity);
            }
        }
    }
}