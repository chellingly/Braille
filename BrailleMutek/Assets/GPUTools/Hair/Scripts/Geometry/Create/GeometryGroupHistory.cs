using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Create
{
    [Serializable]
    public class GeometryGroupHistory
    {
        [SerializeField] private readonly List<List<Vector3>> history = new List<List<Vector3>>();
        [SerializeField] private int pointer;

        public void Record(List<Vector3> list)
        {
            pointer = history.Count;
            history.Add(list.ToList());

            if(history.Count > 10)
                history.RemoveAt(0);
        }

        public List<Vector3> Undo()
        {
            if(pointer > 0)
                pointer--;

            return history[pointer].ToList();
        }

        public List<Vector3> Redo()
        {
            if (pointer < history.Count - 1)
                pointer++;

            return history[pointer].ToList();
        }

        public void Clear()
        {
            history.Clear();
            pointer = 0;
        }

        public bool IsUndo
        {
            get { return history.Count > 0 && pointer > 0; }
        }

        public bool IsRedo
        {
            get { return history.Count > 1 && pointer < history.Count - 1; }
        }
    }
}
