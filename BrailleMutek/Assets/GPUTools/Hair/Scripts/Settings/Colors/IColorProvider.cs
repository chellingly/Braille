namespace GPUTools.Hair.Scripts.Settings.Colors
{
    public interface IColorProvider
    {
        UnityEngine.Color GetColor(HairSettings settings, int x, int y, int sizeY);
    }
}
