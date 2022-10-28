using Leopotam.EcsLite;

namespace Script.Authoring
{
    public interface IAuthoring
    {
        void Convert(EcsWorld world, int entity);
    }
}