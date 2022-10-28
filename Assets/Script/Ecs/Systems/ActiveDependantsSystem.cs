using Leopotam.EcsLite;
using Script.Ecs.Components;

namespace Script.Ecs.Systems
{
    public class ActiveDependantsSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var activateablesPool = world.GetPool<Activateables>();
            var activePool = world.GetPool<Active>();
            
            foreach (var entity in world.Filter<Activateables>().Inc<Active>().End())
            {
                foreach (var dependant in activateablesPool.Get(entity).Objects)
                {
                    if (dependant.TryGetEntity(world, out var dependantEntity))
                    {
                        activePool.GetOrAdd(dependantEntity);
                    }
                }
            }
            
            foreach (var entity in world.Filter<Activateables>().Exc<Active>().End())
            {
                foreach (var dependant in activateablesPool.Get(entity).Objects)
                {
                    if (dependant.TryGetEntity(world, out var dependantEntity))
                    {
                        activePool.Del(dependantEntity);
                    }
                }
            }
        }
    }
}