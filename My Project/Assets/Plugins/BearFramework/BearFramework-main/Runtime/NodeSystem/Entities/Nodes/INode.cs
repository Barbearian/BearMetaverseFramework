using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public interface INode
    {
        public INode GetNode();
    }

    public static class INodeGetter {
        public static INode GetDefaultNode(this INode node) {
            return node;
        }
    }

    public class ANode : INode
    {
        public INode GetNode()
        {
            return this.GetDefaultNode();
        }
    }

    public class ANodeClass: INode{
        public INode GetNode()
        {
            return this.GetDefaultNode();
        }
    }



    public interface INodeData {

    }

    public class ANodeData : INodeData
    {
        private INode _root;
        public INode Root => _root;
    }
}