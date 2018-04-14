using System.Collections;
using System.Collections.Generic;
using Klak.VectorMathExtension;
using Klak.Math;
using UnityEngine;

public class ChangeCameraTargets : MonoBehaviour {

    public enum Interpolator
    {
        Exponential, Spring, DampedSpring
    }


    public Transform target;
    public float speed = 1f;
    public GameObject[] possibleTargets;

    public string keyChange;

    private int  targetIndex;

    int randomTarget;
    Quaternion newRot;
    Vector3 relPos;


    #region Private Properties And Functions

    Vector3 _vposition;
    Vector4 _vrotation;

    Vector3 SpringPosition(Vector3 current, Vector3 target)
    {
        _vposition = ETween.Step(_vposition, Vector3.zero, 1 + speed * 0.5f);
        _vposition += (target - current) * (speed * 0.1f);
        return current + _vposition * Time.deltaTime;
    }

    Quaternion SpringRotation(Quaternion current, Quaternion target)
    {
        var v_current = current.ToVector4();
        var v_target = target.ToVector4();
        _vrotation = ETween.Step(_vrotation, Vector4.zero, 1 + speed * 0.5f);
        _vrotation += (v_target - v_current) * (speed * 0.1f);
        return (v_current + _vrotation * Time.deltaTime).ToNormalizedQuaternion();
    }

    #endregion



    void Start()
    {
        targetIndex = 0;
        if (targetIndex==0)
        {
            for (int i = 0; i < possibleTargets.Length; i++)
            {
               
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        target = possibleTargets[targetIndex].transform;


        Debug.Log(targetIndex);
        if (Input.GetKeyDown(keyChange))
        {
             targetIndex++; 
        }

        if(targetIndex == possibleTargets.Length)
        {
            targetIndex = 0;
        }
        else
        {
          
            relPos = target.position - transform.position;
            newRot = Quaternion.LookRotation(relPos);
            transform.rotation = DTween.Step(transform.rotation, newRot, ref _vrotation, speed);
          
            
        }
    }
 

        
      
    }



   