namespace GPUTools.Common.Scripts.Tools.Commands
{
    public interface IBuildCommand //todo convert to abstract class
    {
        void Build();
        void Dispatch();
        void UpdateSettings();
        void Dispose();
    }
}
