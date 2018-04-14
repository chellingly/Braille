using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools
{
    public class CacheProvider<T> where T : Component
    {
        private readonly List<GameObject> providers;

        public CacheProvider(List<GameObject> providers)
        {
            this.providers = providers;
        }

        private List<T> items;

        public List<T> GetItems()
        {
            var list = new List<T>();

            foreach (var provider in providers)
            {
                if(provider != null)
                    list.AddRange(provider.GetComponentsInChildren<T>().ToList());
            }

            return list;
        }

        public List<T> Items
        {
            get { return items ?? (items = GetItems()); }
        }

        public static bool Verify(List<GameObject> list)
        {
            if (list.Count == 0)
                return false;
            
            foreach (var gameObject in list)
            {
                if (gameObject == null)
                {
                    return false;
                }
            }

            return true;
        }
        
        
    }
}
