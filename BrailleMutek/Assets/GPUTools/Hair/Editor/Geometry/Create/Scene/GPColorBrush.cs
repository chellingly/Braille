using GPUTools.Hair.Scripts.Geometry.Create;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Scene
{
    public class GPColorBrush : GPBaseBrush
    {
        public GPColorBrush(HairGeometryCreator creator) : base(creator, "CSColorBrush")
        {

        }

        public override void StartDrawScene()
        {
            base.StartDrawScene();

            CopyListToArray(Creator.Geomery.Selected.Vertices, Kernel.Vertices.Data);
            Kernel.Vertices.PushData();
        }

        public override void DrawScene()
        {
            CopyListToArray(Creator.Geomery.Selected.Colors, Kernel.Colors.Data);
            Kernel.Colors.PushData();

            Kernel.Dispatch();

            Kernel.Colors.PullData();
            CopyListToArray(Creator.Geomery.Selected.Colors, Kernel.Colors.Data, true);
        }
    }
}
