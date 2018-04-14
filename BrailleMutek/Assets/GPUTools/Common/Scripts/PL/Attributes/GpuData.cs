using System;

namespace GPUTools.Common.Scripts.PL.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GpuData : Attribute
    {
        public string Name;

        public GpuData(string name)
        {
            Name = name;
        }
    }
}