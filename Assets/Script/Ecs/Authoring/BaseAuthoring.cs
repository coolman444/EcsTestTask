using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace Script.Ecs.Authoring
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ConvertToEntity))]
    [Serializable]
    public class BaseAuthoring<T> : MonoBehaviour, IAuthoring where T : struct
    {
        [SerializeField]
        private T _value;

        public void Convert(EcsWorld world, int entity)
        {
            InitComponent(ref _value);
            world.GetPool<T>().Add(entity) = _value;
        }

        protected virtual void InitComponent(ref T value)
        {
        }
    }
}