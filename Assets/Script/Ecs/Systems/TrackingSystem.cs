using System.Collections.Generic;
using Leopotam.EcsLite;
using Script.Ecs.Components;
using UnityEngine;

namespace Script.Ecs.Systems
{
    public class TrackingSystem : IEcsRunSystem
    {
        private readonly ICollisionService _collisionService;

        public TrackingSystem(ICollisionService collisionService)
        {
            _collisionService = collisionService;
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var positionPool = world.GetPool<Position>();
            var trackedEntitiesPool = world.GetPool<TrackedEntities>();
            var trackedByPool = world.GetPool<TrackedBy>();
            var activePool = world.GetPool<Active>();
            var circleTriggerPool = world.GetPool<CircleTrigger>();
            
            foreach (var entity in world.Filter<Velocity>().Inc<Position>().Exc<TrackedBy>().End())
            {
                if (_collisionService.TryFindCollidedEntity(positionPool.Get(entity).Value, out var collidedPackedEntity))
                {
                    if (collidedPackedEntity.Unpack(world, out var collidedEntity))
                    {
                        ref var trackedEntities = ref trackedEntitiesPool.GetOrAdd(collidedEntity);
                        trackedEntities.Entities ??= new List<EcsPackedEntity>();
                        var packedEntity = world.PackEntity(entity);
                        if (!trackedEntities.Entities.Contains(packedEntity))
                        {
                            trackedEntities.Entities.Add(packedEntity);
                        }

                        trackedByPool.Add(entity).Tracker = collidedPackedEntity;
                        activePool.GetOrAdd(collidedEntity);
                    }
                }
            }
            
            foreach (var entity in world.Filter<TrackedEntities>().Inc<CircleTrigger>().Inc<Position>().End())
            {
                var trackedEntities = trackedEntitiesPool.Get(entity).Entities;
                for (var i = trackedEntities.Count - 1; i >= 0; --i)
                {
                    if (!trackedEntities[i].Unpack(world, out var trackedEntity))
                    {
                        trackedEntities.RemoveAt(trackedEntities.Count - 1);
                        continue;
                    }

                    if (Vector3.Distance(positionPool.Get(trackedEntity).Value, positionPool.Get(entity).Value) >= circleTriggerPool.Get(entity).Radius)
                    {
                        trackedEntities.RemoveAt(trackedEntities.Count - 1);
                        trackedByPool.Del(trackedEntity);
                        continue;
                    }
                }

                if (trackedEntities.Count == 0)
                {
                    trackedEntitiesPool.Del(entity);
                    activePool.Del(entity);
                }
            }
        }
    }
}