using Leopotam.EcsLite;

namespace Script.Ecs
{
    public interface IAuthoring
    {
        void Convert(EcsWorld world, int entity);
    }
}