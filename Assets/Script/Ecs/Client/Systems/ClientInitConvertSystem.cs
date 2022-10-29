using Leopotam.EcsLite;
using UnityEngine;

namespace Script.Ecs.Client.Systems
{
    public class ClientInitConvertSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            foreach (var toConvert in Object.FindObjectsOfType<ConvertToEntity>())
            {
                toConvert.Convert(world);
            }
        }
    }
}