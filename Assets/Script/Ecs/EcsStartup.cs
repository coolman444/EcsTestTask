using Leopotam.EcsLite;
using Script.Ecs.Client.Systems;
using Script.Ecs.Systems;
using UnityEngine;

namespace Script.Ecs
{
    public class EcsStartup : MonoBehaviour
    {
        private EcsWorld _world;
        private EcsSystems _ecsFixedUpdateSystems;
        private EcsSystems _ecsUpdateSystems;

        [SerializeField]
        private LayerMask _groundLayers; 

        [SerializeField]
        private float _collisionGridCellSize = 5;
        
        public EcsWorld World => _world;

        private void Awake()
        {
            var collisionService = new CollisionService(_collisionGridCellSize);
            var timeService = new TimeService();
            _world = new EcsWorld();
            _ecsFixedUpdateSystems = new EcsSystems(_world);
            _ecsFixedUpdateSystems
                .Add (new MoveTowardsSystem(timeService))
                .Add (new TrackingSystem(collisionService))
                .Add (new ActiveDependantsSystem())
                .Add (new ActiveDoorSystem())
#if UNITY_EDITOR
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif          
                .Init();
            
            _ecsUpdateSystems = new EcsSystems(_world);
            _ecsUpdateSystems
                .Add(new ClientInitConvertSystem())
                .Add(new ClientInputSystem(_groundLayers))
                .Add(new InitCollisionSystem(collisionService))
                .Add(new ClientUpdateTransformSystem())
                .Add(new ClientUpdateAnimatorSystem())
                .Add(new DestroyEntitySystem())
                .Init();
        }

        private void Update()
        {
            _ecsUpdateSystems.Run();
        }
        
        private void FixedUpdate()
        {
            _ecsFixedUpdateSystems.Run();
        }

        private void OnDestroy()
        {
            _ecsFixedUpdateSystems.Destroy();
            _ecsUpdateSystems.Destroy();
            _world.Destroy();
        }
    }
}
