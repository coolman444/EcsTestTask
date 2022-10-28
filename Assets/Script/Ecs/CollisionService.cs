using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace Script.Ecs
{
    public class CollisionService : ICollisionService
    {
        private struct CollidedBody
        {
            public EcsPackedEntity Entity;
            public Vector3 Position;
            public float Radius;
        }

        private readonly List<CollidedBody> _bodies = new ();
        
        public bool TryFindCollidedEntity(Vector3 position, out EcsPackedEntity entity)
        {
            var bodyIndex = _bodies.FindIndex(b => Vector3.Distance(b.Position, position) < b.Radius);
            if (bodyIndex >= 0)
            {
                entity = _bodies[bodyIndex].Entity; 
                return true;
            }
            
            entity = default;
            return false;
        }

        public void RegisterBody(EcsPackedEntity entity, Vector3 position, float radius)
        {
            _bodies.Add(new CollidedBody
            {
                Position = position,
                Radius = radius,
                Entity = entity,
            });
        }
    }
}