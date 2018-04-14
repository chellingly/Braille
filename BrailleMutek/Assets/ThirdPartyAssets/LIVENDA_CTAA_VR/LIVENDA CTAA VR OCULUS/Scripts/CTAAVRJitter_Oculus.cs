//=====================================
//CTAA VR 1.3 JITTER RENDER OCULUSV201.3A
//Copyright Livenda Labs 2017-2018
//VIRTUAL REALITY VERSION
//=====================================

using System;
using UnityEngine;



[RequireComponent(typeof(Camera))]
public class CTAAVRJitter_Oculus : MonoBehaviour
{
    #region Point distributions

    private static float[] points_Halton_2_3_x8 = new float[16 * 2];
   			   
    private static float HaltonSeq(int prime, int index = 1)
    {
        float r = 0f;
        float f = 1f;
        int i = index;
        while (i > 0)
        {
            f /= prime;
            r += f * (i % prime);
            i = (int)Mathf.Floor(i / (float)prime);
        }
        return r;
    }

    private static void InitializeHalton_2_3(float[] seq)
    {
        for (int i = 0, n = seq.Length / 2; i != n; i++)
        {
            float u = HaltonSeq(2, i + 1) - 0.5f;
            float v = HaltonSeq(3, i + 1) - 0.5f;
            seq[2 * i + 0] = u;
            seq[2 * i + 1] = v;
        }
    }

    static bool _initialized = false;
	static CTAAVRJitter_Oculus()
    {
        if (_initialized == false)
        {
            _initialized = true;
		        
            InitializeHalton_2_3(points_Halton_2_3_x8);
            
        }
    }

    public enum Pattern
    {       
        Halton_2_3_X8,
    };

    private static float[] AccessPointData(Pattern pattern)
    {
        switch (pattern)
        {
            
            case Pattern.Halton_2_3_X8:
                return points_Halton_2_3_x8;
            default:
                Debug.LogError("missing point distribution");
			return points_Halton_2_3_x8;
        }
    }

    public static int AccessLength(Pattern pattern)
    {
        return AccessPointData(pattern).Length / 2;
    }
    #endregion

    private Vector3 focalMotionPos = Vector3.zero;
    private Vector3 focalMotionDir = Vector3.right;

	private Pattern pattern = Pattern.Halton_2_3_X8;
	[Range(0.25f, 0.65f)] public float jitterScale = 0.5f;

    private Vector4 activeSample = Vector4.zero;
	private int activeIndex = -1;
	public Camera.StereoscopicEye VRCameraEYE;

    public Vector2 Sample(Pattern pattern, int index)
    {
        float[] points = AccessPointData(pattern);
        int n = points.Length / 2;
        int i = index % n;

        float x = jitterScale * points[2 * i + 0];
        float y = jitterScale * points[2 * i + 1];
		      
        return new Vector2(x, y);        
    }

	public float getActiveSampleX()
	{
		return activeSample.x;
	}

	public float getActiveSampleY()
	{
		return activeSample.y;
	}

	public static Quaternion ExtractRotationFromMatrix(ref Matrix4x4 matrix) {
		Vector3 forward;
		forward.x = matrix.m02;
		forward.y = matrix.m12;
		forward.z = matrix.m22;

		Vector3 upwards;
		upwards.x = matrix.m01;
		upwards.y = matrix.m11;
		upwards.z = matrix.m21;

		return Quaternion.LookRotation(forward, upwards);
	}

	public static Vector3 ExtractTranslationFromMatrix(ref Matrix4x4 matrix) {
		Vector3 translate;
		translate.x = matrix.m03;
		translate.y = matrix.m13;
		translate.z = matrix.m23;
		return translate;
	}

	private Vector3 WithZ(Vector3 v, float z)
	{
		return new Vector3(v.x, v.y, z);
	}

	private Matrix4x4 GetPerspectiveProjection(float left, float right, float bottom, float top, float near, float far)
	{
		float x = (2.0f * near) / (right - left);
		float y = (2.0f * near) / (top - bottom);
		float a = (right + left) / (right - left);
		float b = (top + bottom) / (top - bottom);
		float c = -(far + near) / (far - near);
		float d = -(2.0f * far * near) / (far - near);
		float e = -1.0f;

		Matrix4x4 m = new Matrix4x4();
		m[0, 0] = x; m[0, 1] = 0; m[0, 2] = a; m[0, 3] = 0;
		m[1, 0] = 0; m[1, 1] = y; m[1, 2] = b; m[1, 3] = 0;
		m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = c; m[2, 3] = d;
		m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = e; m[3, 3] = 0;
		return m;
	}

	private Matrix4x4 GetPerspectiveProjection(Camera camera)
	{
		return GetPerspectiveProjection(camera, 0f, 0f);
	}

	private Matrix4x4 GetPerspectiveProjection(Camera camera, float tsOXp, float tsOYp)
	{
		if (camera == null)
			return Matrix4x4.identity;

		float oneExtentY = Mathf.Tan(0.5f * Mathf.Deg2Rad * camera.fieldOfView);
		float oneExtentX = oneExtentY * camera.aspect;
		float tsXp = oneExtentX / (0.5f * camera.pixelWidth);
		float tsYp = oneExtentY / (0.5f * camera.pixelHeight);
		float oneJitterX = tsXp * tsOXp;
		float oneJitterY = tsYp * tsOYp;

		float cf = camera.farClipPlane;
		float cn = camera.nearClipPlane;
		float xm = (oneJitterX - oneExtentX) * cn;
		float xp = (oneJitterX + oneExtentX) * cn;
		float ym = (oneJitterY - oneExtentY) * cn;
		float yp = (oneJitterY + oneExtentY) * cn;

		return GetPerspectiveProjection(xm, xp, ym, yp, cn, cf);
	}

	private Vector4 GetPerspectiveProjectionCornerRay(Camera camera)
	{
		return GetPerspectiveProjectionCornerRay(camera, 0f, 0f);
	}

	private Vector4 GetPerspectiveProjectionCornerRay(Camera camera, float tsOXp, float tsOYp)
	{
		if (camera == null)
			return Vector4.zero;

		float oneExtentY = Mathf.Tan(0.5f * Mathf.Deg2Rad * camera.fieldOfView);
		float oneExtentX = oneExtentY * camera.aspect;
		float tsXp = oneExtentX / (0.5f * camera.pixelWidth);
		float tsYp = oneExtentY / (0.5f * camera.pixelHeight);
		float oneJitterX = tsXp * tsOXp;
		float oneJitterY = tsYp * tsOYp;

		return new Vector4(oneExtentX, oneExtentY, oneJitterX, oneJitterY);
	}

    void OnPreRender()
    {
        var camera = GetComponent<Camera>();
        if (camera != null && camera.orthographic == false)
        {
            
            {
                Vector3 oldWorld = focalMotionPos;
                Vector3 newWorld = camera.transform.TransformVector(camera.nearClipPlane * Vector3.forward);

                Vector3 oldPoint = (camera.worldToCameraMatrix * oldWorld);
                Vector3 newPoint = (camera.worldToCameraMatrix * newWorld);
				Vector3 newDelta = WithZ((newPoint - oldPoint), 0f);

                var mag = newDelta.magnitude;
                if (mag != 0f)
                {
                    var dir = newDelta / mag;
                    if (dir.sqrMagnitude != 0f)
                    {
                        focalMotionPos = newWorld;
                        focalMotionDir = Vector3.Slerp(focalMotionDir, dir, 0.2f);
                        
                    }
                }
            }

            //Jitter Sampler
            {
                activeIndex += 1;
                activeIndex %= AccessLength(pattern);

                Vector2 sample = Sample(pattern, activeIndex);
                activeSample.z = activeSample.x;
                activeSample.w = activeSample.y;
                activeSample.x = sample.x;
                activeSample.y = sample.y;

				Matrix4x4 matxx = GetPerspectiveProjection(camera, sample.x, sample.y);
				camera.SetStereoProjectionMatrix (VRCameraEYE, matxx);

            }
        }
        else
        {
            activeSample = Vector4.zero;
            activeIndex = -1;
        }


    }

    void OnDisable()
    {
        var camera = GetComponent<Camera>();
        if (camera != null)
        {
            camera.ResetProjectionMatrix();
        }

        activeSample = Vector4.zero;
        activeIndex = -1;
    }

}
