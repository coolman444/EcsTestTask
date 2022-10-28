using Leopotam.EcsLite;
using Script.Ecs.Components;

namespace Script.Ecs.Systems
{
    public class DestroyEntitySystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            foreach (var entity in world.Filter<DestroyEntity>().End())
            {
                world.DelEntity(entity); 
            }
        }
    }
}