namespace Bear
{
    public class ExecutionFilterNodeData : SignalHandlerNodeData, IOnAttachedToNode
    {
        public SignalFilterReceiver filter = new SignalFilterReceiver();
        public void Attached(INode node)
        {
            filter.Output = new ActionNodeSignalReceiver(Execute);

            node.RegisterSignalReceiver<IExecutableNodeSignal>((x) => {
                filter.Receive(x);
            }, true).AddTo(receivers);

            node.RegisterSignalReceiver<AddFilterSignal>((x) => {
                filter.filters.Add(x.filter);
            }, true).AddTo(receivers);

            node.RegisterSignalReceiver<RemoveFilterSignal>((x) => {
                filter.filters.Remove(x.filter);
            }, true).AddTo(receivers);
        }

        public void Execute(INodeSignal executable)
        {
            if (executable is IExecutable exe) {
                exe.Execute();
            }
        }
    }

    public interface IExecutableNodeSignal : IExecutable, INodeSignal { }

    public class AddFilterSignal:INodeSignal {
        public ISignalFilter filter; 
    }

    public class RemoveFilterSignal : INodeSignal
    {
        public ISignalFilter filter;
    }
}