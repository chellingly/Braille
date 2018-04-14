using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Procedural
{
    public class ProceduralScalp : MonoBehaviour
    {
        [SerializeField] public CurveGrid Grid;


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            for (var i = 0; i <= Grid.ViewSizeX; i++)
            {
                for (var j = 0; j <= Grid.ViewSizeY; j++)
                {
                    var localP0 = Grid.GetSplinePoint((float) i/Grid.ViewSizeX, (float) j/Grid.ViewSizeY);
                    var worldP0 = transform.TransformPoint(localP0);
                    //Gizmos.DrawWireSphere(worldP0, 0.01f);

                    if (i < Grid.ViewSizeX)
                    {
                        var localP1 = Grid.GetSplinePoint((float) (i + 1)/Grid.ViewSizeX, (float) j/Grid.ViewSizeY);
                        var worldP1 = transform.TransformPoint(localP1);
                        Gizmos.DrawLine(worldP0, worldP1);
                    }

                    if (j < Grid.ViewSizeY)
                    {
                        var localP1 = Grid.GetSplinePoint((float)i / Grid.ViewSizeX, (float)(j + 1) / Grid.ViewSizeY);
                        var worldP1 = transform.TransformPoint(localP1);
                        Gizmos.DrawLine(worldP0, worldP1);
                    }
                }
            }
        }
    }
}
