using System.Collections.Generic;
using System.Linq;

namespace GPUTools.Common.Scripts.Tools.Debug
{
    public class Validator
    {
        public static bool TestList<T>(List<T> list)
        {
            if (list.Count == 0)
                return false;

            if (list.Any(item => item == null))
            {
                return false;
            }

            return true;
        }
    }
}
