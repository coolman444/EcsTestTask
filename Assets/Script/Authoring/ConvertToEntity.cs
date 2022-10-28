using Leopotam.EcsLite;
using Script.Ecs.Client.Components;
using UnityEngine;

namespace Script.Authoring
{
    [DisallowMultipleComponent]
    public class ConvertToEntity : MonoBehaviour
    {
        private EcsPackedEntity _entity;

        public bool TryGetEntity(EcsWorld world, out int entity)
        {
            return _entity.Unpack(world, out entity);
        }

        public void Convert(EcsWorld world)
        {
            var entity = world.NewEntity();
            _entity = world.PackEntity(entity);
            world.GetPool<ClientGameObjectRef>().Add(entity).Ref = gameObject;
            foreach (var authoring in GetComponents<IAuthoring>())
            {
                authoring.Convert(world, entity);
            }
        }
    }
}