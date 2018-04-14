namespace GPUTools.Common.Scripts.PL.Abstract
{
    public interface IPass
    {
        void Dispatch();
        void Dispose();
    }
}