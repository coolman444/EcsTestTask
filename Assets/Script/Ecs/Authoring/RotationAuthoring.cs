using Script.Ecs.Components;

namespace Script.Ecs.Authoring
{
    public class RotationAuthoring : BaseAuthoring<Rotation>
    {
        protected override void InitComponent(ref Rotation value)
        {
            value.Value = transform.eulerAngles.y;
        }
    }
}