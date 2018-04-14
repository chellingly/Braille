using System.Collections.Generic;
using Assets.GPUTools.Common.Editor.Engine;
using GPUTools.Hair.Scripts.Geometry.Create;
using GPUTools.Hair.Scripts.Geometry.Create.Kernels;
using UnityEngine.Assertions;

namespace Assets.GPUTools.Hair.Editor.Geometry.Create.Scene
{
    public class GPBaseBrush : EditorItemBase
    {
        protected HairGeometryCreator Creator;
        private readonly string kernelName;
        protected HairEditorKernel Kernel;

        public GPBaseBrush(HairGeometryCreator creator, string kernelName = "")
        {
            Creator = creator;
            this.kernelName = kernelName;
        }

        public virtual void StartDrawScene()
        {
            if (Kernel != null)
                Kernel.Dispose();

            var vertices = Creator.Geomery.Selected.Vertices.ToArray();
            var distances = Creator.Geomery.Selected.Distances.ToArray();
            var colors = Creator.Geomery.Selected.Colors.ToArray();

            Kernel = new HairEditorKernel(vertices, colors, distances, Creator, kernelName);
        }

        public void CopyListToArray<T>(List<T> list, T[] array, bool inverce = false)
        {
            Assert.IsTrue(list.Count == array.Length);

            for (var i = 0; i < list.Count; i++)
            {
                if (!inverce)
                {
                    array[i] = list[i];
                }
                else
                {
                    list[i] = array[i];
                }
            }
        }

        public override void Dispose()
        {
            if (Kernel != null)
                Kernel.Dispose();
        }
    }
}
