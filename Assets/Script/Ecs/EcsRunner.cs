using System;
using Leopotam.EcsLite;
using Script.Ecs.Client.Systems;
using Script.Ecs.Systems;

namespace Script.Ecs
{
    public class EcsRunner : IEcsRunner, IDisposable
    {
        private readonly ISystemSpawner _systemSpawner;
        private readonly EcsWorld _world;
        private readonly EcsSystems _ecsFixedUpdateSystems;
        private readonly EcsSystems _ecsUpdateSystems;

        public EcsRunner(ISystemSpawner systemSpawner, EcsWorld world)
        {
            _ecsFixedUpdateSystems = new EcsSystems(world);
            _ecsFixedUpdateSystems
                .DiStart(systemSpawner)
                .Add<MoveTowardsSystem>()
                .Add<TrackingSystem>()
                .Add<ActiveDependantsSystem>()
                .Add<ActiveDoorSystem>()
#if UNITY_EDITOR
                .Add<Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem>()
#endif          
                .DiEnd()
                .Init();
            
            _ecsUpdateSystems = new EcsSystems(world);
            _ecsUpdateSystems
                .DiStart(systemSpawner)
                .Add<ClientInitConvertSystem>()
                .Add<ClientInputSystem>()
                .Add<InitCollisionSystem>()
                .Add<ClientUpdateTransformSystem>()
                .Add<ClientUpdateAnimatorSystem>()
                .Add<DestroyEntitySystem>()
                .DiEnd()
                .Init();
        }

        public void Update()
        {
            _ecsUpdateSystems.Run();
        }
        
        public void FixedUpdate()
        {
            _ecsFixedUpdateSystems.Run();
        }

        public void Dispose()
        {
            _ecsFixedUpdateSystems.Destroy();
            _ecsUpdateSystems.Destroy();
        }
    }
}