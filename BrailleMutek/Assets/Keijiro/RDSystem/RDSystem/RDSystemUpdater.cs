using UnityEngine;
using System.Collections;


namespace RDSystem
{
    public class RDSystemUpdater : MonoBehaviour
    {

    
        #region Editable Properties

        [SerializeField] CustomRenderTexture _texture;
        [SerializeField, Range(1, 16)] int _stepsPerFrame = 4;
        //[SerializeField, Range(0.04f, 0.06f)] float low = 0.06f;
        //[SerializeField, Range(0.04f, 0.06f)] float high = 0.06f;
        //[SerializeField, Range(0, 0.02f)] float oscillate = 0.01f;



        public float Kill
        {
            get { return _Kill; }
            set { _Kill = value; }

        }

        [SerializeField, Range(0, 0.1f)]
        public float _Kill = 0.05f;


        public float Feed
        {
            get { return _Feed; }
            set { _Feed = value; }

        }

        [SerializeField, Range(0, 0.1f)]
        public float _Feed = 0.05f;


        public float Speed
        {
            get { return _Speed; }
            set { _Speed = value; }

        }

        [SerializeField, Range(0, 1)]
        public float _Speed = 0.05f;



        [SerializeField]
        private Material _material;

        public Material Material
        {
            get { return _material; }
            set { _material = value; }
        }


   
        #endregion


       


        [ContextMenu("Reset")]
       
        void Start()
        {
            _texture.Initialize();
        }

        

        void Update()
        {


            //float low = _material.GetFloat("_Kill");
            //float t = Mathf.PingPong(Time.time, oscillate);
            //float _Val = Mathf.Lerp(low, high, t);

            //float _Val = low + Mathf.PingPong(t, high - low);
            _material.SetFloat("_Feed", _Feed);
            _material.SetFloat("_Kill", _Kill);
            _material.SetFloat("_Speed", _Speed);

            _texture.Update(_stepsPerFrame);

        }
    }


}