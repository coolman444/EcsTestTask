using Leopotam.EcsLite;

namespace Script.Ecs
{
    public readonly struct EcsSystemBuilder
    {
        private readonly IEcsSystems _ecsSystems;
        private readonly ISystemSpawner _systemSpawner;

        public EcsSystemBuilder(IEcsSystems ecsSystems, ISystemSpawner systemSpawner)
        {
            _ecsSystems = ecsSystems;
            _systemSpawner = systemSpawner;
        }

        public EcsSystemBuilder Add<T>() where T : class, IEcsSystem
        {
            _ecsSystems.Add(_systemSpawner.SpawnSystem<T>());
            return this;
        }
        
        public IEcsSystems DiEnd()
        {
            return _ecsSystems;
        }
    }
    
    public static class EcsSystemBuilderExtension
    {
        public static EcsSystemBuilder DiStart(this IEcsSystems ecsSystems, ISystemSpawner systemSpawner)
        {
            return new EcsSystemBuilder(ecsSystems, systemSpawner);
        }
    }
}