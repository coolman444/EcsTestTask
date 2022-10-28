using Leopotam.EcsLite;
using Script.Ecs.Components;

namespace Script.Ecs.Systems
{
    public class CollisionDetectSystem : IEcsRunSystem
    {
        private readonly ICollisionService _collisionService;

        public CollisionDetectSystem(ICollisionService collisionService)
        {
            _collisionService = collisionService;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var positionPool = world.GetPool<Position>();
            var collisionEventPool = world.GetPool<CollisionEvent>();
            foreach (var entity in world.Filter<Velocity>().Inc<Position>().End())
            {
                if (_collisionService.TryFindCollidedEntity(positionPool.Get(entity).Value, out var packedEntity))
                {
                    if (packedEntity.Unpack(world, out var collidedEntity))
                    {
                        collisionEventPool.GetOrAdd(collidedEntity).CollidedEntity = world.PackEntity(entity);
                    }
                }
            }
        }
    }
}