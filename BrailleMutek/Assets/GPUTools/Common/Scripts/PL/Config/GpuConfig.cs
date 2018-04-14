namespace GPUTools.Common.Scripts.PL.Config
{
    public class GpuConfig
    {
#if UNITY_STANDALONE_WIN
        public const int NumThreads = 1024;
#endif
#if UNITY_STANDALONE_OSX
        public const int NumThreads = 256;
#endif
    }
}
