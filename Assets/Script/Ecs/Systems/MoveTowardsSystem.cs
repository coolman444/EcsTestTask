using Leopotam.EcsLite;
using Script.Ecs.Components;
using UnityEngine;

namespace Script.Ecs.Systems
{
    public class MoveTowardsSystem : IEcsRunSystem
    {
        private readonly ITimeService _timeService;

        public MoveTowardsSystem(ITimeService timeService)
        {
            _timeService = timeService;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var destinationPool = world.GetPool<DestinationPoint>();
            var characterPool = world.GetPool<Character>();
            var positionPool = world.GetPool<Position>();
            var rotationPool = world.GetPool<Rotation>();
            var velocityPool = world.GetPool<Velocity>();
            var angularRotationPool = world.GetPool<AngularRotation>();
            
            foreach (var entity in world
                         .Filter<Character>()
                         .Inc<Position>()
                         .Inc<Rotation>()
                         .Inc<DestinationPoint>()
                         .End())
            {
                var destination = destinationPool.Get(entity).Value;
                var character = characterPool.Get(entity);
                ref var position = ref positionPool.Get(entity);
                ref var rotation = ref rotationPool.Get(entity);
                
                if (Vector3.Distance(destination, position.Value) < character.StopDistance)
                {
                    destinationPool.Del(entity);
                    velocityPool.Del(entity);
                    angularRotationPool.Del(entity);
                    continue;
                }

                var moveDirection = destination - position.Value;
                var targetRotation = Vector3.SignedAngle(Vector3.forward, moveDirection, Vector3.up);
                var rotateAngle = targetRotation - rotation.Value;
                if (Mathf.Abs(rotateAngle) < character.MoveAngle)
                {
                    velocityPool.GetOrAdd(entity).Value = moveDirection.normalized * character.LinearSpeed; 
                }
                else
                {
                    velocityPool.Del(entity);
                }

                if (!Mathf.Approximately(rotateAngle, 0))
                {
                    ref var angularRotation = ref angularRotationPool.GetOrAdd(entity);  
                    angularRotation.Speed = character.AngularSpeed;
                    angularRotation.TargetRotation = targetRotation;
                }
                else
                {
                    angularRotationPool.Del(entity);
                }
            }
            
            foreach (var entity in world
                         .Filter<Velocity>()
                         .Inc<Position>()
                         .Inc<DestinationPoint>()
                         .End())
            {
                var destination = destinationPool.Get(entity).Value;
                var velocity = velocityPool.Get(entity);
                ref var position = ref positionPool.Get(entity);
                
                positionPool.Get(entity).Value = Vector3.MoveTowards(position.Value, destination, velocity.Value.magnitude * _timeService.DeltaTime);
            }
            
            foreach (var entity in world
                         .Filter<AngularRotation>()
                         .Inc<Rotation>()
                         .End())
            {
                var angularRotation = angularRotationPool.Get(entity);
                ref var rotation = ref rotationPool.Get(entity);
                
                if (!Mathf.Approximately(angularRotation.TargetRotation, rotation.Value))
                {
                    rotation.Value = Mathf.MoveTowardsAngle(rotation.Value, angularRotation.TargetRotation, angularRotation.Speed * _timeService.DeltaTime);
                }
            }
        }
    }
}