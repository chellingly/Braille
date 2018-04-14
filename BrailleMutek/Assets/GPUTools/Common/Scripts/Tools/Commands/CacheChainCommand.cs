using System.Collections.Generic;

namespace GPUTools.Common.Scripts.Tools.Commands
{
    public class CacheChainCommand : ICacheCommand
    {
        private readonly List<ICacheCommand> commands = new List<ICacheCommand>();

        public void Cache()
        {
            foreach (var command in commands)
                command.Cache();

            OnCache();
        }

        protected void Add(ICacheCommand command)
        {
            commands.Add(command);
        }

        protected virtual void OnCache()
        {
            
        }
    }
}
