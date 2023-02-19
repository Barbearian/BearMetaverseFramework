using cfg.Graph;
using System.Collections.Generic;

namespace Bear
{
    public static class SignalFactory
    {
        
        public static Dictionary<int, cfg.Graph.Node> NodePool = new Dictionary<int, cfg.Graph.Node>();
        public static INodeSignal MakeNode(this cfg.Graph.Node node) {
            
            return null;
        }

        public static cfg.Graph.Node ToNode(this int index) {
            if (NodePool.TryGetValue(index,out var node)) {
                return node;
            }
            return null;
        }

        public static void InitSignalFactory(this TbNodeGraph graph) {
            foreach (var item in graph.DataList)
            {
                NodePool[item.NodeIndex] = item;
            }
        }
    }
}