using System;

namespace Script.Ecs.Components
{
    [Serializable]
    public struct Character
    {
        public float StopDistance;
        public float MoveAngle;
        public float LinearSpeed;
        public float AngularSpeed;
    }
}