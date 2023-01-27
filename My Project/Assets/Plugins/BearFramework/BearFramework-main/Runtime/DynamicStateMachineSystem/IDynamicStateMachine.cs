using System.Collections.Generic;

namespace Bear
{
    public interface IDynamicTransition{
        public bool CanTransit(INodeSignal signal);
        public void Transit(DynamicStateMachine machine, INodeSignal signal);
    }

    public class DynamicStateMachine:INodeSignalReceiver {
        public List<IDynamicTransition> Transitions { get; set; } = new List<IDynamicTransition>();
        private bool _isActive = true;
        public bool IsActive { get => _isActive; set => _isActive = value; }

        public void ClearTransition() {
            Transitions = new List<IDynamicTransition>();
        }

        public void Receive(INodeSignal signal)
        {
            for (int i = 0; i < Transitions.Count; i++)
            {
                var transition = Transitions[i];
                if (transition.CanTransit(signal)) {
                    ClearTransition();
                    transition.Transit(this,signal);
                    return;
                }
            }
        }
    }
}