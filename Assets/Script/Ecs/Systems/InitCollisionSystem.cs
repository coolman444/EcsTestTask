using Leopotam.EcsLite;
using Script.Ecs.Client.Components;
using Script.Ecs.Components;

namespace Script.Ecs.Systems
{
    public class InitCollisionSystem : IEcsInitSystem
    {
        private readonly ICollisionService _collisionService;
        
        public InitCollisionSystem(ICollisionService collisionService)
        {
            _collisionService = collisionService;
        }

        public void Init(IEcsSystems systems)
        {
            
            var world = systems.GetWorld();

            var positionPool = world.GetPool<Position>();
            var circleTriggerPool = world.GetPool<CircleTrigger>();
            var collisionBodyPool = world.GetPool<CollisionBody>();
            
            foreach (var entity in world.Filter<CircleTrigger>().Inc<Position>().Exc<CollisionBody>().End())
            {
                _collisionService.RegisterBody(
                    world.PackEntity(entity), 
                    positionPool.Get(entity).Value, 
                    circleTriggerPool.Get(entity).Radius);
                
                collisionBodyPool.Add(entity);
            }
        }
    }
}