
using System.Collections.Generic;

namespace Bear
{
    public class ActionNodeData :INodeData
    { 
        public INode Launcher { get; set; }
        public List<INode> Affected { get; set; } = new List<INode>();
    }

    public class PreActSignal : INodeSignal { }
    public class ActSignal : INodeSignal { }
    public class PostActSignal : INodeSignal { }

    public class ActionCancelSignal : INodeSignal { }
}