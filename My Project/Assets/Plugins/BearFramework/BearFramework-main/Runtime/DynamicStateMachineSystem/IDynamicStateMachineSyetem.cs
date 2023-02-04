using JetBrains.Annotations;
using System.Collections.Generic;
using System;
namespace Bear
{
    public static class DynamicStateMachineFactory {

        public static DynamicGraph MakeStateMachine(DynamicGraphData graphData, Func<TransitionData, IDynamicStateTransition> TransitionMaker) {

            DynamicGraph graph= new DynamicGraph();
            Dictionary<int, INode> states = new Dictionary<int, INode>();
            var transitions = graphData.edges;
            var root = graphData.source;

            //add edges
            foreach (var transition in transitions)
            {
                //Create Node
                var source = states.GetOrCreateNode(transition.source);
                var edge = TransitionMaker(transition);
                if (!source.TryGetNodeData<DynamicStateTranstionNodeData>(out var transitionsData))
                {
                    transitionsData = source.AddNodeData(new DynamicStateTranstionNodeData());

                }
                edge.Target = states.GetOrCreateNode(transition.target);
                transitionsData.nextState.Add(edge);
            }

            graph.nodes = states;
            graph.enterNode = graphData.source;
            return graph;
        }

        private static INode GetOrCreateNode(this Dictionary<int, INode> states, int index)
        {
            if (!states.TryGetValue(index, out var rs))
            {
                rs = new CNode();
                states[index] = rs;
            }
            return rs;
        }

        public static IDynamicStateTransition BuildIntTransition(TransitionData transition)
        {
            return new IntTransition()
            {
                key = transition.key,
            };
        }
    }


    [System.Serializable]
    public struct TransitionData {
        public int source;
        public int target;
        public int key;
    }



    [System.Serializable]
    public struct DynamicGraphData
    {
        public int source;
        public TransitionData[] edges;
        public string[] nodeKeys;
        
    }

    public class DynamicGraph
    {
        public Dictionary<int, INode> nodes { get; set; } = new Dictionary<int, INode>();
        public int enterNode { get; set; }
    }

    public struct IntTransition : IDynamicStateTransition
    {
        public int key { get; set; }
        public INode Target { get; set; }

        public bool CanTransit(INodeSignal signal)
        {
            if (signal is IKeyTransitionSignal<int> itSignal) {
                return itSignal.Value == key;
            }
            return false;
        }


    }

    public interface IKeyTransitionSignal<T> : INodeSignal {
        public T Value { get; set; }
    }

    public struct IntKeyTransitionSignal : IKeyTransitionSignal<int>
    {
        public int Value { get ; set; }
    }
}