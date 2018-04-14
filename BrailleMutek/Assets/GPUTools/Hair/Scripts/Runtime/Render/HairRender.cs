using GPUTools.Hair.Scripts.Runtime.Data;
using GPUTools.Hair.Scripts.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace GPUTools.Hair.Scripts.Runtime.Render
{
    public class HairRender : MonoBehaviour
    {
        private Mesh mesh;
        private HairDataFacade data;
        private MeshRenderer rend;
        

        private void Awake()
        {
            mesh = new Mesh();
            rend = gameObject.AddComponent<MeshRenderer>();
            gameObject.AddComponent<MeshFilter>().mesh = mesh;
        }

        public void Initialize(HairDataFacade data)
        {
            this.data = data;

            InitializeMaterial();
            InitializeMesh();
        }

        private void InitializeMesh()
        {
            mesh.vertices = new Vector3[(int)data.Size.x];
            mesh.SetIndices(data.Indices, MeshTopology.Triangles, 0);
        }

        private void InitializeMaterial()
        {
            rend.material = Resources.Load<Material>("Materials/Hair");
            //old
            //rend.material.SetBuffer("_BarycentricBuffer", data.Barycentrics.ComputeBuffer);
            //rend.material.SetBuffer("_BodiesDataBuffer", data.ParticlesData.ComputeBuffer);
            //rend.material.SetBuffer("_BodiesBuffer", data.Particles.ComputeBuffer);
            //
            rend.material.SetBuffer("_Barycentrics", data.Barycentrics.ComputeBuffer);
            rend.material.SetBuffer("_Particles", data.TessRenderParticles.ComputeBuffer);

           
        }

        public void Dispatch()
        {
            UpdateBounds();
            UpdateMaterial();
            UpdateRenderer();
        }

        private void UpdateBounds()
        {
            mesh.bounds = transform.InverseTransformBounds(data.Bounds);
        }

        private void UpdateMaterial()
        {
            rend.material.SetVector("_LightCenter", data.LightCenter);
            rend.material.SetVector("_TessFactor", data.TessFactor);
            rend.material.SetFloat("_StandWidth", data.StandWidth);

            rend.material.SetFloat("_SpecularShift", data.SpecularShift);
            rend.material.SetFloat("_PrimarySpecular", data.PrimarySpecular);
            rend.material.SetFloat("_SecondarySpecular", data.SecondarySpecular);
            rend.material.SetColor("_SpecularColor", data.SpecularColor);

            rend.material.SetFloat("_Diffuse", 0/*1 - data.Diffuse*/);
            rend.material.SetFloat("_FresnelPower", data.FresnelPower);
            rend.material.SetFloat("_FresnelAtten", data.FresnelAttenuation);

            rend.material.SetVector("_WavinessAxis", data.WavinessAxis);

            rend.material.SetVector("_Length", data.Length);
            rend.material.SetFloat("_Volume", data.Volume);

            rend.material.SetVector("_Size", data.Size);
        }

        private void UpdateRenderer()
        {
            rend.shadowCastingMode = data.CastShadows ? ShadowCastingMode.On : ShadowCastingMode.Off;
            rend.receiveShadows = data.ReseiveShadows;
        }
        
        /*private void OnBecameInvisible() 
        {
            Debug.Log("OnBecameInvisible");
        }*/

        public bool IsVisible
        {
            get { return rend.isVisible; } 
        }
    }
}
