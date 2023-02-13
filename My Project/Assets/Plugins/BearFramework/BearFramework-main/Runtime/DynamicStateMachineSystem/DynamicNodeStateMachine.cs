using Mono.Cecil.Cil;
using System.Collections.Generic;
using UnityEngine;

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

    public class DynamicStateMachineNodeData : INodeData,IOnAttachedToNode { 
        public INode Entry { get; set; }
        public INode Current { get; set; }
        public INode Predicted { get; set; }
        private INode Root { get; set; }

        public DynamicGraph StateMachine;

        public void Executed() {
            Current = Predicted;
            Predicted = null;
        }

        public void Init(DynamicGraph states)
        {
            this.StateMachine = states;
            if (states.nodes.TryGetValue(states.enterNode,out var current)) {
                this.Entry = current;
                this.Current = current;
            }
            
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

        public void EnterDefault() {
            Current = Entry;
        }

        public void EnterState(int index) {
            if (StateMachine.nodes.TryGetValue(index, out var node)) {
                Debug.Log("Entering state "+index);
                EnterState(node);
            }
        }

        public void EnterState(INode node) {
            //if (node.TryGetNodeData<DynamicStateExecutionNodeData>(out var data)) {
            //    Root.ReceiveNodeSignal(data.executionSignal);
            //}

            Current = node;
        }

        public void Attached(INode node)
        {
            Root = node;

            
        }
    }

    public interface IDynamicStateTransition { 
        public INode Target { get; set; }
        public bool CanTransit(INodeSignal signal);
    }

    


}