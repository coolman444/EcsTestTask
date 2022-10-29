using Leopotam.EcsLite;
using Zenject;

namespace Script.Ecs
{
    public class SystemSpawner : ISystemSpawner
    {
        private readonly DiContainer _container;

        public SystemSpawner(DiContainer container)
        {
            _container = container;
        }

        public T SpawnSystem<T>() where T : class, IEcsSystem
        {
            return _container.Instantiate<T>();
        }
    }
}