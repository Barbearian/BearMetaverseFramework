

using System.Collections.Generic;

namespace Bear
{
    public class SignalFilterReceiver: INodeSignalReceiver
    {
        private bool _isActive = true;
        public bool IsActive { get => _isActive; set { _isActive = value; } }
        public INodeSignalReceiver Output { get; set; }
        public List<ISignalFilter> filters { get; set; } = new List<ISignalFilter>();

        public void Receive(INodeSignal signal)
        {
            foreach (var filter in filters)
            {
                if (!filter.CanPass(signal)) {
                    return;
                }
            }

            Output.Receive(signal);
        }

        public void Clear() {
            filters = new List<ISignalFilter>();
        }
    }

    public interface ISignalFilter {
        public bool CanPass(INodeSignal signal = null);
    }


}