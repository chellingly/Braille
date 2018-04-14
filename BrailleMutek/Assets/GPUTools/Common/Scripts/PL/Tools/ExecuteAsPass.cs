using System;
using GPUTools.Common.Scripts.PL.Abstract;

namespace GPUTools.Common.Scripts.PL.Tools
{
    public class ExecuteAsPass : IPass
    {
        private readonly Action action;

        public ExecuteAsPass(Action action)
        {
            this.action = action;
        }

        public void Dispatch()
        {
            action();
        }

        public void Dispose()
        {
            
        }
    }
}
