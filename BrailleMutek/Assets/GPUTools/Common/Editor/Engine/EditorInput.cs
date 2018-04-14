using System.Collections.Generic;
using UnityEngine;

namespace Assets.GPUTools.Common.Editor.Engine
{
    public class EditorInput
    {
        private readonly List<KeyCode> keyCodes = new List<KeyCode>();
        private readonly List<KeyCode> downKeyCodes = new List<KeyCode>();
        private readonly List<KeyCode> upKeyCodes = new List<KeyCode>();
        private bool mouseButton;

        public void Update()
        {
            downKeyCodes.Clear();
            upKeyCodes.Clear();

            var e = Event.current;

            if (e.type == EventType.KeyDown)
            {
                if (!keyCodes.Contains(e.keyCode))
                {
                    keyCodes.Add(e.keyCode);
                    downKeyCodes.Add(e.keyCode);
                }
            }

            if (e.type == EventType.KeyUp)
            {
                if (keyCodes.Contains(e.keyCode))
                {
                    keyCodes.Remove(e.keyCode);
                    upKeyCodes.Add(e.keyCode);
                }
            }

            if (e.type == EventType.MouseDown)
                mouseButton = true;
            
            if (e.type == EventType.MouseUp)
                mouseButton = false;
        }

        public bool GetKey(KeyCode code)
        {
            return keyCodes.Contains(code);
        }

        public bool GetKeyDown(KeyCode code)
        {
            return downKeyCodes.Contains(code);
        }

        public bool GetKeyUp(KeyCode code)
        {
            return upKeyCodes.Contains(code);
        }

        public bool GetMouseButton()
        {
            return mouseButton;
        }
        
        public bool GetMouseButtonDown()
        {
            return Event.current.type == EventType.MouseDown;
        }
        
        public bool GetMouseButtonUp()
        {
            return Event.current.type == EventType.MouseUp;
        }
    }
}
