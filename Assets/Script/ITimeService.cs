namespace Script
{
    public interface ITimeService
    {
        public float FrameTime { get; }
        public float DeltaTime { get; }
    }
}