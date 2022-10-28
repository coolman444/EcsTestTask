using UnityEngine;

namespace Script
{
    public class TimeService : ITimeService
    {
        public float FrameTime => Time.time;
        public float DeltaTime => Time.deltaTime;
    }
}