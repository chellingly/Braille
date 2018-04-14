using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Render
{
    public class BuildBarycentrics : IBuildCommand
    {
        public static int MaxCount = 64;
        
        private readonly HairSettings settings;

        private FixedList<Vector3> barycentric = new FixedList<Vector3>(MaxCount);
        //private int factor = 0;
        //private int oldN = int.MaxValue;
        
        public BuildBarycentrics(HairSettings settings)
        {
            this.settings = settings;
        }

        
        public void Build()
        {
            //factor = settings.LODSettings.GetDetail(settings.StandsSettings.HeadCenterWorld);
            settings.RuntimeData.Barycentrics = new GpuBuffer<Vector3>(barycentric.Data, sizeof(float) * 3);
            Gen();
        }

        public void Dispatch()
        {
            /*var newFactor = settings.LODSettings.GetDetail(settings.StandsSettings.HeadCenterWorld);
            if (newFactor != factor)
            {
                factor = newFactor;
                //Gen();
            }*/
        }

        public void UpdateSettings()
        {
            //factor = settings.LODSettings.GetDetail(settings.StandsSettings.HeadCenterWorld);
            Gen(true);
        }

        public void Dispose()
        {
            settings.RuntimeData.Barycentrics.Dispose();
        }

        private void Gen(bool forceUpdate = false)
        {
            /*var n = 1;
            var off = 0.2f;
            if (factor >= 15)
            {
                off = 0.1f;
                n = 2;
            }
            if (factor >= 45)
            {
                off = 0.05f;
                n = 3;
            }
            
            if(n == oldN && !forceUpdate)
                return;*/

            var off = 0.1f;
            var n = 2;
            
            //oldN = n;
            var m = 1 - off;
            var mm = (1 - m) * 0.5f;
            
            barycentric.Reset();
            Split(new Vector3(m, mm, mm), new Vector3(mm, m, mm), new Vector3(mm, mm, m), n);

            while (barycentric.Count < MaxCount)
            {
                var k = GetRandomK();
                if (!barycentric.Contains(k))
                    barycentric.Add(GetRandomK());
            }
            
            settings.RuntimeData.Barycentrics.PushData();
        }

        private void Split(Vector3 b1, Vector3 b2, Vector3 b3, int steps)
        {
            steps--;

            TryAdd(b1);
            TryAdd(b2);
            TryAdd(b3);

            var n1 = (b1 + b2) * 0.5f;
            var n2 = (b2 + b3) * 0.5f;
            var n3 = (b3 + b1) * 0.5f;

            if (steps < 0)
                return;

            Split(b1, n1, n3, steps);
            Split(b2, n1, n2, steps);
            Split(b3, n2, n3, steps);
            Split(n1, n2, n3, steps);
        }

        private void TryAdd(Vector3 v)
        {
            if (!barycentric.Contains(v))
            {             
                barycentric.Add(v);
            }
        }


        private Vector3 GetRandomK()
        {
            var ka = Random.Range(0f, 1f);
            var kb = Random.Range(0f, 1f);

            if (ka + kb > 1)
            {
                ka = 1 - ka;
                kb = 1 - kb;
            }

            var kc = 1 - (ka + kb);
            return new Vector3(ka, kb, kc);
        }
    }
}
