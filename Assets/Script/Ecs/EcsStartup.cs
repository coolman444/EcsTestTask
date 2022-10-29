using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Script.Ecs
{
    public class EcsStartup : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _groundLayers; 

        [SerializeField]
        private float _collisionGridCellSize = 5;

        private DiContainer _container;
        private IEcsRunner _ecsRunner;
        private EcsWorld _world;

        private void Awake()
        {
            _container = new DiContainer();
            _container.BindInstance(new CollisionServiceSettings
            {
                CollisionGridCellSize = _collisionGridCellSize,
            }); 
            
            _container.BindInstance(new ClientSettings
            {
                GroundLayers = _groundLayers,
            });

            _world = new EcsWorld();
            _container.BindInstance(_world).AsSingle().NonLazy();
            _container.Bind<DisposableManager>().AsSingle().NonLazy();
            _container.BindInterfacesTo<SystemSpawner>().AsSingle().NonLazy();
            _container.BindInterfacesTo<EcsRunner>().AsSingle().NonLazy();
            _container.BindInterfacesTo<CollisionService>().AsSingle().NonLazy();
            _container.BindInterfacesTo<UnityTimeService>().AsSingle().NonLazy();

            _container.ResolveRoots();
            _ecsRunner = _container.Resolve<IEcsRunner>();
        }

        private void Update()
        {
            _ecsRunner.Update();
        }
        
        private void FixedUpdate()
        {
            _ecsRunner.FixedUpdate();
        }

        private void OnDestroy()
        {
            _container.Resolve<DisposableManager>().Dispose();
            _world.Destroy();
        }
    }
}
