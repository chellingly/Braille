using GPUTools.Hair.Scripts.Geometry.Create;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Scene
{
    public class GPShrinkBrush : GPBaseBrush
    {
        private readonly float speed;

        public GPShrinkBrush(HairGeometryCreator creator, float speed) : base(creator, "CSShrinkBrush")
        {
            this.speed = speed;
        }

        public override void DrawScene()
        {
            CopyListToArray(Creator.Geomery.Selected.Vertices, Kernel.Vertices.Data);
            Kernel.Vertices.PushData();

            CopyListToArray(Creator.Geomery.Selected.Distances, Kernel.Distances.Data);
            Kernel.Distances.PushData();

            Kernel.BrushLengthSpeed.Value = speed;

            Kernel.Dispatch();

            Kernel.Vertices.PullData();
            CopyListToArray(Creator.Geomery.Selected.Vertices, Kernel.Vertices.Data, true);

            Kernel.Distances.PullData();
            CopyListToArray(Creator.Geomery.Selected.Distances, Kernel.Distances.Data, true);
        }
    }
}
