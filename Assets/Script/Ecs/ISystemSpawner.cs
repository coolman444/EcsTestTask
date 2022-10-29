using Leopotam.EcsLite;

namespace Script.Ecs
{
    public interface ISystemSpawner
    {
        T SpawnSystem<T>() where T : class, IEcsSystem;
    }
}