using Leopotam.EcsLite;
using Script.Ecs.Components;

namespace Script.Ecs
{
    public static class EcsExtensions
    {
        public static ref T GetOrAdd<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (pool.Has(entity))
            {
                return ref pool.Get(entity);
            }
            
            return ref pool.Add(entity);
        }
    }
}