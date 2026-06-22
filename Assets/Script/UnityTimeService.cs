using UnityEngine;

namespace Script
{
    public class UnityTimeService : ITimeService
    {
        public int Test3;
        public float FrameTime => Time.time;
        public float DeltaTime => Time.deltaTime;
    }
}