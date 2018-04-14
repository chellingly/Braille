namespace GPUTools.Common.Scripts.PL.Tools
{
    public class GpuValue<T>
    {
        public T Value { set; get; }

        public GpuValue(T value = default(T))
        {
            Value = value;
        }
    }
}
