using System.Collections.Generic;
using NUnit.Framework.Constraints;

namespace Assets.GPUTools.Common.Editor.Engine
{
    public class Processor : EditorItemBase
    {
        private List<EditorItemBase> items = new List<EditorItemBase>(); 

        public void Add(EditorItemBase item)
        {
            items.Add(item);
        }

        public override void DrawInspector()
        {
            foreach (var item in items)
            {
                item.DrawInspector();
                if(!item.Validate())
                    return;
            }
        }

        public override void DrawScene()
        {
            foreach (var item in items)
            {
                item.DrawScene();
                if (!item.Validate())
                    return;
            }
        }

        public override void Dispose()
        {
            items.ForEach(i => i.Dispose());
            items.Clear();
        }
        
        
    }
}
