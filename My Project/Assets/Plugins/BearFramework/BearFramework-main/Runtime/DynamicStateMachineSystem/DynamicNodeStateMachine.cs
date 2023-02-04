using Mono.Cecil.Cil;
using System.Collections.Generic;

namespace Bear
{
    public class DynamicStateNodeData : INodeData, IOnAttachedToNode {
        public CNode root { get; set; }
        public List<INodeSignal> DOnEnterSignal { get; set; } = new List<INodeSignal>();
        public List<INodeSignal> DOnExitSignal { get; set; } = new List<INodeSignal>();
        //   private bool inited { get; set; }
        public void Attached(INode node)
        {
            if (node is CNode cnode) {
                Init(cnode);
            }

        }

        private void Init(CNode node) {
            root = node;
            //  inited = true; 
        }


    }

    public class DynamicStateExecutionNodeData : INodeData {
        public INodeSignal executionSignal { get; set; }
    }

    public class DynamicStateTranstionNodeData : INodeData {
        public List<IDynamicStateTransition> nextState { get; set; } = new List<IDynamicStateTransition>();
        public bool Predict(INodeSignal signal, out INode target)
        {
            for (int i = 0; i < nextState.Count; i++)
            {
                var transition = nextState[i];
                if (transition.CanTransit(signal))
                {
                    target = transition.Target;
                    return true;
                }
            }

            target = null;
            return false;
        }

    }

    public class DynamicStateMachineNodeData : INodeData { 
        public INode Entry { get; set; }
        public INode Current { get; set; }
        public INode Predicted { get; set; }

        public void Executed() {
            Current = Predicted;
            Predicted = null;
        }

        public bool TryGetPredictedSignal(out INodeSignal signal) {
            if (Predicted != null) {
                if (Predicted.TryGetNodeData<DynamicStateExecutionNodeData>(out var data)) {
                    signal = data.executionSignal;
                    return true;
                }
            }

            signal = null;
            return false;
        }

        public bool TryPredict(INodeSignal signal) {
            if (Current != null) {
                if (Current.TryGetNodeData<DynamicStateTranstionNodeData>(out var data)) {
                    var rs =  data.Predict(signal, out var predicted);
                    Predicted = predicted;
                    return rs;
                }
            }

            return false;
        }
    }

    public interface IDynamicStateTransition { 
        public INode Target { get; set; }
        public bool CanTransit(INodeSignal signal);
    }

    


}