using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools
{
    [Serializable]
    public class SerializableDictionary<TK, TV>
    {
        [SerializeField] private List<TK> keys = new List<TK>();
        [SerializeField] private List<TV> values = new List<TV>();

        public void Add(TK key, TV value)
        {
            if (keys.Contains(key))
                throw new Exception("Key already added");

            keys.Add(key);
            values.Add(value);
        }

        public TV GetValue(TK key)
        {
            if(!keys.Contains(key))
                throw new Exception("Can't found key");

            return values[keys.IndexOf(key)];
        }
    }
}
