using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Tools
{
    public class GpuMatrix4x4
    {
        public Matrix4x4 Data { get; set; }

        private float[] values = new float[16];
        
        public GpuMatrix4x4(Matrix4x4 data)
        {
            Data = data;
        }

        public float[] Values
        {
            get
            {
                values[0] = Data[0];
                values[1] = Data[1];
                values[2] = Data[2];
                values[3] = Data[3];
                values[4] = Data[4];
                values[5] = Data[5];
                values[6] = Data[6];
                values[7] = Data[7];
                values[8] = Data[8];
                values[9] = Data[9];
                values[10] = Data[10];
                values[11] = Data[11];
                values[12] = Data[12];
                values[13] = Data[13];
                values[14] = Data[14];
                values[15] = Data[15];

                return values;
            }
        }
    }
}