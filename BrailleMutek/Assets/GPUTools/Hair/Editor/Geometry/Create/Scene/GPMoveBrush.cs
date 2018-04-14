using GPUTools.Hair.Scripts.Geometry.Create;
using UnityEngine;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Scene
{
    public class GPMoveBrush : GPBaseBrush
    {
        public GPMoveBrush(HairGeometryCreator creator) : base(creator, "CSMoveBrush")
        {
            
        }

        public override void StartDrawScene()
        {
            base.StartDrawScene();

            Creator.Brush.OldPosition = Creator.Brush.Position;

            CopyListToArray(Creator.Geomery.Selected.Distances, Kernel.Distances.Data);
            Kernel.Distances.PushData();
        }

        public override void DrawScene()
        {
            CopyListToArray(Creator.Geomery.Selected.Vertices, Kernel.Vertices.Data);
            Kernel.Vertices.PushData();

            Kernel.Dispatch();

            Kernel.Vertices.PullData();
            CopyListToArray(Creator.Geomery.Selected.Vertices, Kernel.Vertices.Data, true);
        }
    }
}
