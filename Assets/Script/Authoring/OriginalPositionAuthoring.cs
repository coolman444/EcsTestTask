using Script.Ecs.Components;

namespace Script.Authoring
{
    public class OriginalPositionAuthoring : BaseAuthoring<OriginalPosition>
    {
        protected override void InitComponent(ref OriginalPosition value)
        {
            value.Value = transform.position;
        }
    }
}