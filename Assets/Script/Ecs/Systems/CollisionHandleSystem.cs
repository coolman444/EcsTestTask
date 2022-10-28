using Leopotam.EcsLite;
using Script.Ecs.Components;

namespace Script.Ecs.Systems
{
    public class CollisionHandleSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var collisionEventPool = world.GetPool<CollisionEvent>();
            var activePool = world.GetPool<Active>();
            foreach (var entity in world.Filter<CollisionEvent>().End())
            {
                activePool.GetOrAdd(entity);
                collisionEventPool.Del(entity);
            }
        }
    }
}