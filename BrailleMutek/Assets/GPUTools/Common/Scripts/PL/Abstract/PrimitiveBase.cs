using System;
using System.Collections.Generic;
using System.Reflection;
using GPUTools.Common.Scripts.PL.Attributes;
using UnityEngine;

namespace GPUTools.Common.Scripts.PL.Abstract
{
    public class PrimitiveBase : IPass
    {
        private readonly List<IPass> passes = new List<IPass>();

        protected void Bind()
        {
            CachePassAttributes();
            CacheOwnAttributes();
            BindAttributes();
        }

        public virtual void Dispatch()
        {
            for (var i = 0; i < passes.Count; i++)
                passes[i].Dispatch();
        }

        public virtual void Dispose()
        {
            for (var i = 0; i < passes.Count; i++)
                passes[i].Dispose();
        }

        public void AddPass(IPass pass)
        {
            passes.Add(pass);
        }

        public void RemovePass(IPass pass)
        {
            if (!passes.Contains(pass))
            {
                Debug.LogError("Can't find pass");
                return;
            }

            passes.Remove(pass);
        }

        #region reflection
        
        private readonly List<List<KeyValuePair<GpuData, PropertyInfo>>> passesAttributes = new List<List<KeyValuePair<GpuData, PropertyInfo>>>();
        private readonly List<KeyValuePair<GpuData, PropertyInfo>> ownAttributes = new List<KeyValuePair<GpuData, PropertyInfo>>();

        private void CachePassAttributes()
        {
            passesAttributes.Clear();

            for (var i = 0; i < passes.Count; i++)
            {
                var pass = passes[i];
                var passProperties = pass.GetType().GetProperties();

                var passAttributes = new List<KeyValuePair<GpuData, PropertyInfo>>();
                passesAttributes.Add(passAttributes);

                for (var j = 0; j < passProperties.Length; j++)
                {
                    var passProperty = passProperties[j];
                    if (!Attribute.IsDefined(passProperty, typeof(GpuData)))
                        continue;

                    var passAttribute = (GpuData)Attribute.GetCustomAttribute(passProperty, typeof(GpuData));
                    passAttributes.Add(new KeyValuePair<GpuData, PropertyInfo>(passAttribute, passProperty));
                }
            }
        }

        private void CacheOwnAttributes()
        {
            ownAttributes.Clear();

            var properties = GetType().GetProperties();

            for (var i = 0; i < properties.Length; i++)
            {
                var propertyInfo = properties[i];
                if (!Attribute.IsDefined(propertyInfo, typeof(GpuData)))
                    continue;
                var attribute = (GpuData)Attribute.GetCustomAttribute(propertyInfo, typeof(GpuData));
                ownAttributes.Add(new KeyValuePair<GpuData, PropertyInfo>(attribute, propertyInfo));
            }
        }

        protected void BindAttributes()
        {
            for (var i = 0; i < ownAttributes.Count; i++)
            {
                var ownAttribute = ownAttributes[i];

                for (var j = 0; j < passesAttributes.Count; j++)
                {
                    var passAttributes = passesAttributes[j];
                    for (var k = 0; k < passAttributes.Count; k++)
                    {
                        var passAttribute = passAttributes[k];
                        if (passAttribute.Key.Name.Equals(ownAttribute.Key.Name))
                        {
                            passAttribute.Value.SetValue(passes[j], ownAttribute.Value.GetValue(this, null), null);
                        }
                    }
                }
            }
        }

        #endregion
    }
}