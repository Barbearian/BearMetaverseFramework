
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bear
{
    public interface ICustomizedNode : INode, IDisposable
    {
        //Node 
        public void AddChildrenNode(INode kid);
        public void RemoveChildrenNode(INode kid);
        public bool TryGetKidNode<T>(out T kid) where T : INode;
        public bool TryGetKidNode<T>(int index, out T kid) where T : INode;
        public bool TryGetKidNode(int index, out INode kid);
        public INode[] GetAllKids();
        public bool TryGetParentNode(out INode parent);


        //Node Data
        //Add
        public T AddNodeData<T>(T data) where T : INodeData;

        //Remove
        public T RemoveNodeData<T>() where T : INodeData;
        public void RemoveNodeData(INodeData data);
        public void RemoveAllNodeData();

        //Fetch

        public bool RequestNodeData<T>(Action<T> DOnDataRequested) where T : INodeData;
        public void InvokeRequst(INodeData data);


        public bool TryGetNodeData<T>(out T data) where T : INodeData;
        public INodeData[] GetAllNodeData();
        public bool TryGetFirstNodeDataInChildren<T>(out T data) where T : INodeData;
        public bool TryGetFirstNodeDataInParent<T>(out T data) where T : INodeData;
        public T GetOrCreateNodeData<T>(T defaultNode) where T : INodeData;
        public T GetOrCreateNodeData<T>() where T : INodeData;
    }

    public class CNode : ICustomizedNode
    {
        public INode parent;
        private List<INode> kids = new List<INode>();
        private Dictionary<Type, INodeData> nodeData = new Dictionary<Type, INodeData>();
        Dictionary<Type, List<Action<INodeData>>> requests = new Dictionary<Type, List<Action<INodeData>>>();

        public void AddChildrenNode(INode kid)
        {
            if (kid is ICustomizedNode ckid) {
                if (ckid.TryGetParentNode(out var parent)) {
                    parent.RemoveChildrenNode(ckid);
                }
            }

            kids.Add(kid);
        }

        public void Dispose()
        {
            if (parent != null)
            {
                parent.RemoveChildrenNode(this);
            }

            RemoveAllNodeData();

            foreach (var kid in kids)
            {
                kid.Dispose();
            }

            kids.Clear();


        }

        private void RemoveNodeData(INodeData dote,bool removeFromList = true) {
            dote.OnDetached(this);
            if (removeFromList) {
                nodeData.Remove(dote.GetType());
            }
        }

        public INode[] GetAllKids()
        {
            return kids.ToArray();
        }

        public INode GetNode()
        {
            return this;
        }

        public void RemoveChildrenNode(INode kid)
        {
            kids.Remove(kid);
            NodeDataLifeCycleSystem.OnRemoveChildren(this, kid);

        }

        public bool TryGetKidNode<T>(out T kid) where T : INode
        {

            kid = default;
            foreach (var item in kids)
            {
                if (item is T target)
                {
                    kid = target;
                    return true;
                }
            }
            return false;
        }

        public bool TryGetKidNode<T>(int index, out T kid) where T : INode
        {
            kid = default;
            if (index >= 0 && index < kids.Count)
            {
                if (kids[index] is T rs)
                {
                    kid = rs;
                    return true;
                }

            }
            return false;
        }

        public bool TryGetKidNode(int index, out INode kid)
        {
            kid = default;
            if (index >= 0 && index < kids.Count)
            {
                kid = kids[index];
                return true;
            }
            return false;
        }

        public bool TryGetParentNode(out INode parent)
        {
            parent = this.parent; 
            return parent != null;
        }



        public T AddNodeData<T>(T data) where T : INodeData
        {
            var key = data.GetType();
            nodeData[key] = data;

            data.OnAttched(this);

            //Invoke Request
            InvokeRequst(data);
            return data;
        }

        public T RemoveNodeData<T>() where T : INodeData
        {
            if (TryGetNodeData<T>(out var nodedata))
            {
                RemoveNodeData(nodedata);
            }
            return nodedata;

        }

        public void RemoveNodeData(INodeData data)
        {
            var key = data.GetType();
            nodeData.Remove(key);

            data.OnDetached(this);
        }

        public void RemoveAllNodeData()
        {
            foreach (var dote in nodeData.Values)
            {
                RemoveNodeData(dote, false);
            }

            nodeData.Clear();
        }

        public void InvokeRequst(INodeData data)
        {
            var key = data.GetType();
            var NodeDataRequests = this.requests;
            if (NodeDataRequests.TryGetValue(key, out var requests))
            {
                foreach (var request in requests)
                {
                    request.Invoke(data);
                }

                NodeDataRequests.Remove(key);
            }
        }

        public bool RequestNodeData<T>(Action<T> DOnDataRequested) where T : INodeData
        {
            var key = typeof(T);
            var NodeData = nodeData;
            var NodeDataRequests = requests;
            if (NodeData.TryGetValue(key, out var value))
            {
                if (value is T tNodeData)
                {
                    DOnDataRequested?.Invoke(tNodeData);
                }

            }
            else
            {
                NodeDataRequests.Enqueue(
                    key,

                    (nodeData) => {
                        if (nodeData is T tNodeData)
                        {
                            DOnDataRequested?.Invoke(tNodeData);
                        }

                    });
            }
            return true;
        }

        public bool TryGetNodeData<T>(out T data) where T : INodeData
        {

            if (nodeData.TryGetValue(typeof(T), out var ndata))
            {
                data = (T)ndata;
                return true;
            }

            data = default;
            return false;
        }

        public INodeData[] GetAllNodeData()
        {
            return nodeData.Values.ToArray();
        }

        public bool TryGetFirstNodeDataInChildren<T>(out T data) where T : INodeData
        {
            var kids = GetAllKids();
            foreach (var kid in kids)
            {
                if (kid.TryGetNodeData(out data))
                {
                    return true;
                }
            }

            data = default;
            return false;
        }

        public bool TryGetFirstNodeDataInParent<T>(out T data) where T : INodeData
        {
            if (TryGetParentNode(out var parent))
            {
                if (parent.TryGetNodeData(out data))
                {
                    return true;
                }
                else
                {
                    return parent.TryGetFirstNodeDataInParent(out data);
                }
            }

            data = default;
            return false;
        }

        public T GetOrCreateNodeData<T>(T defaultNode) where T : INodeData
        {
            if (TryGetNodeData<T>(out T data))
            {
                return data;
            }
            else
            {

                return (T)AddNodeData(defaultNode);
            }
        }

        public T GetOrCreateNodeData<T>() where T : INodeData
        {
            if (TryGetNodeData<T>(out T data))
            {
                return data;
            }
            else
            {

                var defaultNode = (T)Activator.CreateInstance(typeof(T));
                return (T)AddNodeData(defaultNode);
            }
        }


    }
}