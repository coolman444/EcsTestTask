using Leopotam.EcsLite;
using UnityEngine;

namespace Script.Ecs
{
    public interface ICollisionService
    {
        bool TryFindCollidedEntity(Vector3 position, out EcsPackedEntity entity);
        void RegisterBody(EcsPackedEntity entity, Vector3 position, float radius);
    }
}