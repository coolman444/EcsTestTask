using Leopotam.EcsLite;
using Script.Ecs.Client.Components;
using Script.Ecs.Components;
using UnityEngine;

namespace Script.Ecs.Client.Systems
{
    public class ClientUpdateAnimatorSystem : IEcsRunSystem
    {
        private static readonly int Speed = Animator.StringToHash("Speed");

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var velocityPool = world.GetPool<Velocity>();
            var animatorPool = world.GetPool<ClientAnimatorRef>();
            
            foreach (var entity in world.Filter<ClientAnimatorRef>().Inc<Velocity>().End())
            {
                var animator = animatorPool.Get(entity).Animator;
                animator.SetFloat(Speed, velocityPool.Get(entity).Value.magnitude);
            }
            
            foreach (var entity in world.Filter<ClientAnimatorRef>().Exc<Velocity>().End())
            {
                var animator = animatorPool.Get(entity).Animator;
                animator.SetFloat(Speed, 0);
            }
        }
    }
}