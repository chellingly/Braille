using System;
using GPUTools.Hair.Scripts.Geometry.Create;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Scene
{
    public class GPRemoveBrush : GPBaseBrush
    {
        public GPRemoveBrush(HairGeometryCreator creator) : base(creator, "CSRemoveBrush")
        {

        }

        public override void StartDrawScene()
        {
            base.StartDrawScene();

            CopyListToArray(Creator.Geomery.Selected.Vertices, Kernel.Vertices.Data);
            Kernel.Vertices.PushData();

            CopyListToArray(Creator.Geomery.Selected.Distances, Kernel.Distances.Data);
            Kernel.Distances.PushData();
        }

        public override void DrawScene()
        {
            Kernel.Dispatch();

            Kernel.Distances.PullData();
            CopyListToArray(Creator.Geomery.Selected.Distances, Kernel.Distances.Data, true);

            TestForRemove();
        }

        private void TestForRemove()
        {
            var deleted = false;

            var vertices = Creator.Geomery.Selected.Vertices;
            var colors = Creator.Geomery.Selected.Colors;
            var distances = Creator.Geomery.Selected.Distances;

            for (var i = distances.Count - 1; i >= 0; i -= 1)
            {
                if (Math.Abs(distances[i]) < 0.00000000001f)
                {
                    vertices.RemoveRange(i * Creator.Segments, Creator.Segments);
                    colors.RemoveRange(i * Creator.Segments, Creator.Segments);
                    distances.RemoveAt(i);
                    deleted = true;
                }
            }

            if (deleted && Creator.Geomery.Selected.Vertices.Count > 0)
            {
                StartDrawScene();
            }
        }

    }
}
