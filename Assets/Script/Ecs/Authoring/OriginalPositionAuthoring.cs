using Script.Ecs.Components;

namespace Script.Ecs.Authoring
{
    public class OriginalPositionAuthoring : BaseAuthoring<OriginalPosition>
    {
        protected override void InitComponent(ref OriginalPosition value)
        {
            value.Value = transform.position;
        }
    }
}