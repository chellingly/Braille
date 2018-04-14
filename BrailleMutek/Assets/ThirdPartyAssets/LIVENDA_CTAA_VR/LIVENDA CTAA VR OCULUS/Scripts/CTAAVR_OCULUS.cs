//===============================================================
//LIVENDA CTAA VR OCULUS - CINEMATIC TEMPORAL ANTI ALIASING
//VIRTUAL REALITY VERSION 1.3 MP
//Copyright Livenda Labs 2017-2018
//===============================================================

using UnityEngine;
using System.Collections;


[RequireComponent (typeof(Camera))]
[RequireComponent (typeof(CTAAVRJitter_Oculus))]
[RequireComponent (typeof(CTAAVelocity_Oculus))]
[AddComponentMenu("Livenda Effects/CTAAVR_OCULUS")]
public class CTAAVR_OCULUS : MonoBehaviour {

	public bool CTAA_Enabled 			= true;

	[Header("CTAA Settings")]

	[Range(1, 8)] public float TemporalStability     	   = 1.5f;
	[Space(5)]
	[Range(0.0f, 0.2f)] public float AdaptiveEnhance       = 0.0f;


	private bool  PreEnhanceEnabled   	= true;
	private float preEnhanceStrength 	= 1.0f;
	private float preEnhanceClamp    	= 0.005f;
	private float AdaptiveResolve       = 3000.0f;
	private float TemporalQuality 	 	= 1.5f;

	private int forwardMode;


	//#######################################################################################
	//#######################################################################################

	private CTAAVelocity_Oculus _velocity;
	private Camera _cam;


	void Awake()
	{
		_cam = GetComponent<Camera> ();
		_velocity = GetComponent<CTAAVelocity_Oculus>();

	}
	//#######################################################################################
	//#######################################################################################


	private Material mat_txaa;
	private Material mat_enhance;
	private RenderTexture rtAccum0;
	private RenderTexture rtAccum1;
	private RenderTexture txaaOut;
	private RenderTexture afterPreEnhace;
	private bool firstFrame;
	private bool swap;

	private static Material CreateMaterial(string shadername)
	{
		if (string.IsNullOrEmpty(shadername)) 
		{
			return null;
		}
		Material material = new Material(Shader.Find(shadername));
		material.hideFlags = HideFlags.HideAndDontSave;
		return material;
	}
	
	private static void DestroyMaterial(Material mat)
	{
		if (mat != null)
		{
			Object.DestroyImmediate(mat);
			mat = null;
		}
	}


	private void OnEnable()
	{
		_cam = GetComponent<Camera> ();
		firstFrame   = true;
		swap	     = true;

		CreateMaterials();

		if (_cam.actualRenderingPath == RenderingPath.Forward) {
			forwardMode = 1;
		} else {
			forwardMode = 0;
		}

	}

	private void OnDisable()
	{		
		DestroyImmediate(rtAccum0);      	   rtAccum0       		= null;
		DestroyImmediate(rtAccum1);    	 	   rtAccum1       		= null;
		DestroyImmediate(txaaOut);       	   txaaOut        		= null;
		DestroyImmediate(afterPreEnhace);      afterPreEnhace       = null;
		DestroyMaterial(mat_txaa);
		DestroyMaterial(mat_enhance);
	}


	private void CreateMaterials()
	{
		if (mat_txaa == null)      	  	  mat_txaa     	  	  = CreateMaterial("Hidden/CTAAVR_Oculus");
		if (mat_enhance == null)   	  	  mat_enhance  	  	  = CreateMaterial("Hidden/AdaptiveEnhanceVR_Oculus");
	}


	void SetCTAA_Parameters()
	{		
		TemporalQuality = TemporalStability;
		PreEnhanceEnabled   	= AdaptiveEnhance > 0.01 ? true : false;
		preEnhanceStrength 		= Mathf.Lerp (0.2f, 1.5f, 	  AdaptiveEnhance);
		preEnhanceClamp    	    = Mathf.Lerp (0.005f, 0.008f, AdaptiveEnhance);
		AdaptiveResolve         = 3000.0f;
	}
	
	void Start () 
	{
		
		_cam = GetComponent<Camera> ();
		_cam.depthTextureMode = DepthTextureMode.Depth;


		if (_cam.actualRenderingPath == RenderingPath.Forward) {
			forwardMode = 1;
		} else {
			forwardMode = 0;
		}

		CreateMaterials();

		SetCTAA_Parameters ();

		StartCoroutine (fixCam ());

	}

	IEnumerator fixCam()
	{
		_cam = GetComponent<Camera> ();

		if (_cam.actualRenderingPath == RenderingPath.Forward) {

			_cam.renderingPath = RenderingPath.DeferredShading;

			yield return new WaitForSeconds (0.5f);

			_cam.renderingPath = RenderingPath.Forward;
		} 

		yield return new WaitForSeconds (0.1f);
	}


	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{

		SetCTAA_Parameters ();
				
		CreateMaterials();


			if (((rtAccum0 == null) || (rtAccum0.width != source.width)) || (rtAccum0.height != source.height))
			{
				DestroyImmediate(rtAccum0);
				rtAccum0 = new RenderTexture(source.width, source.height, 0, source.format);
				rtAccum0.hideFlags  = HideFlags.HideAndDontSave;
				rtAccum0.filterMode = FilterMode.Bilinear;
				rtAccum0.wrapMode   = TextureWrapMode.Clamp;
				
			}

			if (((rtAccum1 == null) || (rtAccum1.width != source.width)) || (rtAccum1.height != source.height))
			{
				DestroyImmediate(rtAccum1);
				rtAccum1 = new RenderTexture(source.width, source.height, 0, source.format);
				rtAccum1.hideFlags  = HideFlags.HideAndDontSave;
				rtAccum1.filterMode = FilterMode.Bilinear;
				rtAccum1.wrapMode   = TextureWrapMode.Clamp;
			}

			if (((txaaOut == null) || (txaaOut.width != source.width)) || (txaaOut.height != source.height))
			{
				DestroyImmediate(txaaOut);
				txaaOut = new RenderTexture(source.width, source.height, 0, source.format);
				txaaOut.hideFlags  = HideFlags.HideAndDontSave;
				txaaOut.filterMode = FilterMode.Bilinear;
				txaaOut.wrapMode   = TextureWrapMode.Clamp;
			}

			if (((afterPreEnhace == null) || (afterPreEnhace.width != source.width)) || (afterPreEnhace.height != source.height))
			{
				DestroyImmediate(afterPreEnhace);
				afterPreEnhace = new RenderTexture(source.width, source.height, 0, source.format);
				afterPreEnhace.hideFlags  = HideFlags.HideAndDontSave;
				afterPreEnhace.filterMode = source.filterMode;
				afterPreEnhace.wrapMode   = TextureWrapMode.Clamp;
			}


			//-----------------------------------------------------------
			if(PreEnhanceEnabled)
			{
				mat_enhance.SetFloat("_DELTAXp", 1.0f / (float)Screen.width);
				mat_enhance.SetFloat("_DELTAYp", 1.0f / (float)Screen.height);
				mat_enhance.SetFloat("_Strength", preEnhanceStrength);
				mat_enhance.SetFloat("_DELTAMAXC", 	  preEnhanceClamp);

				Graphics.Blit(source, afterPreEnhace, mat_enhance, 1);
			}
			else
			{
				Graphics.Blit(source, afterPreEnhace);
			}
			//-----------------------------------------------------------


			if(CTAA_Enabled)
			{
				
			mat_txaa.SetFloat ("_RenderPath", (float)forwardMode);


				if (firstFrame)
				{
					Graphics.Blit(afterPreEnhace, rtAccum0);
					firstFrame = false;
				}

				mat_txaa.SetTexture("_Motion0",    	    _velocity.velocityBuffer );
				mat_txaa.SetFloat("_AdaptiveResolve",   AdaptiveResolve);
				float tempqual = (float)TemporalQuality;
				mat_txaa.SetVector("_ControlParams", new Vector4(0 , tempqual , 0 , 0) );

				if (swap)
				{
					mat_txaa.SetTexture("_Accum", rtAccum0);
					Graphics.Blit(afterPreEnhace, rtAccum1, mat_txaa);	
					Graphics.Blit(rtAccum1, txaaOut);
				}
				else
				{
					mat_txaa.SetTexture("_Accum", rtAccum1);
					Graphics.Blit(afterPreEnhace, rtAccum0, mat_txaa);
					Graphics.Blit(rtAccum0, txaaOut);
				}

				//CTAA VR OUT		
				Graphics.Blit(txaaOut, destination);

			}//End Of CTAA Enabled
			else
			{
				Graphics.Blit(afterPreEnhace, destination);
			}


			swap = !swap;
	

	}

}
