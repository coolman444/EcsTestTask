using Script.Ecs.Components;

namespace Script.Ecs.Authoring
{
    public class PositionAuthoring : BaseAuthoring<Position>
    {
        protected override void InitComponent(ref Position value)
        {
            value.Value = transform.position;
        }
    }
}