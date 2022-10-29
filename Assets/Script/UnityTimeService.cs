using UnityEngine;

namespace Script
{
    public class UnityTimeService : ITimeService
    {
        public float FrameTime => Time.time;
        public float DeltaTime => Time.deltaTime;
    }
}