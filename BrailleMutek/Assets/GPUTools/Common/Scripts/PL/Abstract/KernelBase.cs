using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Common.Scripts.PL.Abstract
{
    public class KernelBase : IPass
    {
        public bool IsEnabled { get; set; }

        protected readonly int KernelId;

        protected ComputeShader Shader { get; private set; }

        public KernelBase(string shaderPath, string kernelName)
        {
            Shader = Resources.Load<ComputeShader>(shaderPath);
            Assert.IsNotNull(Shader, "Can't load shader");

            KernelId = Shader.FindKernel(kernelName);
            IsEnabled = true;
        }

        public virtual void Dispatch()
        {
            if (!IsEnabled)
                return;

            if(Props.Count == 0)
                CacheAttributes();

            BindAttributes();
            Shader.Dispatch(KernelId, GetGroupsNumX(), GetGroupsNumY(), GetGroupsNumZ());
        }

        public virtual void Dispose()
        {
        }

        public virtual int GetGroupsNumX()
        {
            return 1;
        }

        public virtual int GetGroupsNumY()
        {
            return 1;
        }

        public virtual int GetGroupsNumZ()
        {
            return 1;
        }

        #region Reflection

        protected readonly List<KeyValuePair<GpuData, object>> Props = new List<KeyValuePair<GpuData, object>>(); 

        protected virtual void CacheAttributes()
        {
            Props.Clear();

            var properties = GetType().GetProperties();

            foreach (var propertyInfo in properties)
            {
                if (!Attribute.IsDefined(propertyInfo, typeof(GpuData)))
                    continue;

                var attribute = (GpuData) Attribute.GetCustomAttribute(propertyInfo, typeof(GpuData));
                var obj = propertyInfo.GetValue(this, null);
                Props.Add(new KeyValuePair<GpuData, object>(attribute, obj));
            }
        }

        protected void BindAttributes()
        {
            for (var i = 0; i < Props.Count; i++)
            {
                var attribute = Props[i].Key;
                var obj = Props[i].Value;

                if (obj is IBufferWrapper)
                {
                    var buffer = ((IBufferWrapper) obj).ComputeBuffer;
                    Shader.SetBuffer(KernelId, attribute.Name, buffer);
                    Shader.SetInt(attribute.Name + "Length", buffer.count);
                }
                else if (obj is Texture)
                    Shader.SetTexture(KernelId, attribute.Name, (Texture) obj);
                else if (obj is GpuValue<int>)
                    Shader.SetInt(attribute.Name, ((GpuValue<int>)obj).Value);
                else if (obj  is GpuValue<float>)
                    Shader.SetFloat(attribute.Name, ((GpuValue<float>)obj).Value);
                else if (obj is GpuValue<Vector3>)
                    Shader.SetVector(attribute.Name, ((GpuValue<Vector3>)obj).Value);
                else if (obj is GpuValue<Color>)
                    Shader.SetVector(attribute.Name, ((GpuValue<Color>)obj).Value.ToVector());
                else if (obj is GpuValue<bool>)
                    Shader.SetBool(attribute.Name, ((GpuValue<bool>)obj).Value);
                else if (obj is GpuValue<GpuMatrix4x4>)
                    Shader.SetFloats(attribute.Name, ((GpuValue<GpuMatrix4x4>) obj).Value.Values);
            }
        }

        #endregion
    }
}