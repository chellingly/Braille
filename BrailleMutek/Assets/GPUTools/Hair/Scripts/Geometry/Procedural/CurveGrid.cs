using System;
using GPUTools.Common.Scripts.Tools;
using GPUTools.Common.Scripts.Utils;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Procedural
{
    [Serializable]
    public class CurveGrid
    {
        [SerializeField] public Vector3[] ControlPoints;
        [SerializeField] public int ControlSizeX;
        [SerializeField] public int ControlSizeY;

        //[SerializeField] Vector3[] ViewPoints;
        [SerializeField] public int ViewSizeX;
        [SerializeField] public int ViewSizeY;

        public void GenerateControl()
        {
            ControlPoints = new Vector3[ControlSizeX*ControlSizeY];

            for (int x = 0; x < ControlSizeX; x++)
            {
                for (int y = 0; y < ControlSizeY; y++)
                {
                    var valueX = (float)x/ControlSizeX;
                    var valueY = (float)y/ControlSizeY;

                    SetControl(x, y, new Vector3(valueX, 0, valueY));
                }
            }
        }

        public void GenerateView()
        {
            //ViewPoints = new Vector3[ViewSizeX*ViewSizeY];

            for (var x = 0; x < ViewSizeX; x++)
            {
                for (var y = 0; y < ViewSizeY; y++)
                {
                    var tX = (float) x/ViewSizeX;
                    var tY = (float) y/ViewSizeY;

                    GetSplinePoint(tX, tY);
                }
            }
        }

        public Vector3 GetSplinePoint(float tX, float tY)
        {
            int iX = (int)(tX*ControlSizeX);
            int i0X = Mathf.Max(0, iX - 1);
            int i1X = Mathf.Min(iX, ControlSizeX - 1);
            int i2X = Mathf.Min(iX + 1, ControlSizeX - 1);

            int iY = (int)(tY*ControlSizeY);
            int i0Y = Mathf.Max(0, iY - 1);
            int i1Y = Mathf.Min(iY, ControlSizeY - 1);
            int i2Y = Mathf.Min(iY + 1, ControlSizeY - 1);

            var p00 = GetControl(i0X, i0Y);
            var p10 = GetControl(i1X, i0Y);
            var p20 = GetControl(i2X, i0Y);

            var p01 = GetControl(i0X, i1Y);
            var p11 = GetControl(i1X, i1Y);
            var p21 = GetControl(i2X, i1Y);

            var p02 = GetControl(i0X, i2Y);
            var p12 = GetControl(i1X, i2Y);
            var p22 = GetControl(i2X, i2Y);

            var cPoint10 = (p00 + p10) * 0.5f;
            var cPoint20 = (p10 + p20) * 0.5f;

            var cPoint11 = (p01 + p11) * 0.5f;
            var cPoint21 = (p11 + p21) * 0.5f;

            var cPoint12 = (p02 + p12) * 0.5f;
            var cPoint22 = (p12 + p22) * 0.5f;

            float tStepX = 1.0f / ControlSizeX;
            float localTx = (tX % tStepX) * ControlSizeX;
            var resultX0 = CurveUtils.GetBezierPoint(cPoint10, p10, cPoint20, localTx);
            var resultX1 = CurveUtils.GetBezierPoint(cPoint11, p11, cPoint21, localTx);
            var resultX2 = CurveUtils.GetBezierPoint(cPoint12, p12, cPoint22, localTx);

            var cPoint1Y = (resultX0 + resultX1)*0.5f;
            var cPoint2Y = (resultX2 + resultX1)*0.5f;

            float tStepY = 1.0f / ControlSizeY;
            float localTy = (tY % tStepY) * ControlSizeY;

            return CurveUtils.GetBezierPoint(cPoint1Y, resultX1, cPoint2Y, localTy);
        }

        public void SetControl(int x, int y, Vector3 value)
        {
            ControlPoints[x*ControlSizeY + y] = value;
        }

        public Vector3 GetControl(int x, int y)
        {
            return ControlPoints[x*ControlSizeY + y];
        }

        public void SetView(int x, int y, Vector3 value)
        {
            ControlPoints[x*ViewSizeY + y] = value;
        }
        public Vector3 GetView(int x, int y)
        {
            return ControlPoints[x*ViewSizeX + y];
        }
    }
}
