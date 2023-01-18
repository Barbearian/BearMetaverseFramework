using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear {
    public interface IOnAttachedToNode {
        public void Attached(INode node);
    }

    public interface IOnDetachedFromNode {
        public void Detached(INode node);
    }

    public interface IOnAddChildren{
        public void AddedChild(INode kid);
    }

    public interface IOnRemoveChildren
    {
        public void RemoveChild(INode kid);
    }

    public interface IOnChangeParent {
        public void ChangedParent(INode parent);
    }

    public static class NodeDataLifeCycleSystem{
        public static void OnAttched(this INodeData nodedata, INode root){
            if(nodedata is IOnAttachedToNode idata && root != null){
                idata.Attached(root);
            }
        }

        public static void OnDetached(this INodeData nodedata, INode root){
            if(nodedata is IOnDetachedFromNode idata && root != null){
                idata.Detached(root);
            }
        }

        public static void OnAddChildren(INode parent,INode kid) {
            foreach (var item in parent.GetAllNodeData())
            {
                if (item is IOnAddChildren addedChildren) { 
                    addedChildren.AddedChild(kid);
                }
            }

            foreach (var item in kid.GetAllNodeData())
            {
                if (item is IOnChangeParent ChangedParent)
                {
                    ChangedParent.ChangedParent(parent);
                }
            }
        }

        public static void OnRemoveChildren(INode parent, INode kid)
        {
            foreach (var item in parent.GetAllNodeData())
            {
                if (item is IOnRemoveChildren addedChildren)
                {
                    addedChildren.RemoveChild(kid);
                }
            }

           
        }
    }
}