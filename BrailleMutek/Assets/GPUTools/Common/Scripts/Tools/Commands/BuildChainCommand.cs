using System.Collections.Generic;

namespace GPUTools.Common.Scripts.Tools.Commands
{
    public class BuildChainCommand : IBuildCommand
    {
        private readonly List<IBuildCommand> commands = new List<IBuildCommand>();

        public void Add(IBuildCommand command)
        {
            commands.Add(command);
        }

        public void Build()
        {
            for (var i = 0; i < commands.Count; i++)
                commands[i].Build();

            OnBuild();
        }

        public void UpdateSettings()
        {
            for (var i = 0; i < commands.Count; i++)
                commands[i].UpdateSettings();

            OnUpdateSettings();
        }

        public void Dispatch()
        {
            for (var i = 0; i < commands.Count; i++)
                commands[i].Dispatch();

            OnDispatch();
        }

        public void Dispose()
        {
            for (var i = 0; i < commands.Count; i++)
                commands[i].Dispose();

            OnDispose();
        }

        protected virtual void OnBuild()
        { 

        }

        protected virtual void OnUpdateSettings()
        {

        }

        protected virtual void OnDispatch()
        {

        }

        protected virtual void OnDispose()
        {

        }
    }
}
