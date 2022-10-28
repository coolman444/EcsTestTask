using System;
using Script.Authoring;

namespace Script.Ecs.Components
{
    [Serializable]
    public struct Activateables
    {
        public ConvertToEntity[] Objects;
    }
}