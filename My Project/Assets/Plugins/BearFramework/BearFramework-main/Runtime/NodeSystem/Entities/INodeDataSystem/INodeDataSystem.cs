using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Bear{
    public static class INodeDataSystem 
    {
        public static Dictionary<INode,NodeInfo> nodeInfo = new Dictionary<INode, NodeInfo>();
        internal static Dictionary<Type,INodeData> GetOrCreateNodeDataCollection(this INode node){



            if (nodeInfo.TryGetValue(node,out NodeInfo info)){
                return info.nodeData;
            }else{
                NodeInfo ninfo = new NodeInfo(){
                    nodeData = new Dictionary<Type, INodeData>(),
                    requests = new Dictionary<Type, List<Action<INodeData>>>()         
                };
                nodeInfo[node] = ninfo;
                return ninfo.nodeData;
            }
        }



        public static Dictionary<Type,List<Action<INodeData>>> GetNodeDataRequestCollection(this INode node){
            if(nodeInfo.TryGetValue(node,out NodeInfo info)){
                return info.requests;
            }else{
                NodeInfo ninfo = new NodeInfo(){
                    nodeData = new Dictionary<Type, INodeData>(),
                    requests = new Dictionary<Type, List<Action<INodeData>>>()         
                };
                nodeInfo[node] = ninfo;
                return ninfo.requests;
            }
        }

        public static void InvokeRequst(INode node,INodeData data){

            if (node is ICustomizedNode CNode)
            {
                CNode.InvokeRequst(data);
                return;
            }

            var key = data.GetType();
            var NodeDataRequests = node.GetNodeDataRequestCollection();
            if (NodeDataRequests.TryGetValue(key, out var requests))
            {
                foreach (var request in requests)
                {
                    request.Invoke(data);
                }

                NodeDataRequests.Remove(key);
            }
        }
    }

    public static class INodeDataAttachSystem{
        public static T AddNodeData<T>(this INode node, T data) where T : INodeData
        {
            if (node is ICustomizedNode CNode) {
                return CNode.AddNodeData(data);
            }

            var key = data.GetType();
            var NodeData = node.GetOrCreateNodeDataCollection();
            

            
            NodeData[key] = data;

            data.OnAttched(node);


            //Invoke Request
            INodeDataSystem.InvokeRequst(node,data);


            return data;
        }
    }

    public static class INodeDataDetachSystem{


        public static T RemoveNodeData<T>(this INode node) where T:INodeData
        {
            if (node is ICustomizedNode CNode)
            {
                return CNode.RemoveNodeData<T>();
            }

            if (node.TryGetNodeData<T>(out var nodedata)){
                node.RemoveNodeData(nodedata);
           }
           return nodedata;
        }

        public static void RemoveNodeData(this INode node, INodeData data){
            if (node is ICustomizedNode CNode)
            {
                CNode.RemoveNodeData(data);
                return;
            }

            var key = data.GetType();
            if(INodeDataSystem.nodeInfo.TryGetValue(node,out var info)){
                info.nodeData.Remove(key);      
            }       

            data.OnDetached(node);     
        }

        public static void RemoveAllNodeData(this INode node){
            if (node is ICustomizedNode CNode)
            {
                CNode.RemoveAllNodeData();
                return;
            }

            if (INodeDataSystem.nodeInfo.TryGetValue(node,out var info)){
                var allData = info.nodeData;
                INodeDataSystem.nodeInfo.Remove(node);

                foreach(var nodedata in allData.Values){
                    node.RemoveNodeData(nodedata);
                }
            }
        }
    }

    public static class INodeDataFetchSystem{
        public static bool RequestNodeData<T>(this INode node,System.Action<T> DOnDataRequested) where T:INodeData{

            if (node is ICustomizedNode CNode)
            {
                return CNode.RequestNodeData<T>(DOnDataRequested);
                
            }

            var key = typeof(T);
            var NodeData = node.GetOrCreateNodeDataCollection();
            var NodeDataRequests = node.GetNodeDataRequestCollection();
            if (NodeData.TryGetValue(key, out var value))
            {
                if (value is T tNodeData) {
                    DOnDataRequested?.Invoke(tNodeData);
                }
                
            }
            else {
                NodeDataRequests.Enqueue(
                    key,
                    
                    (nodeData)=> {
                    if (nodeData is T tNodeData)
                    {
                        DOnDataRequested?.Invoke(tNodeData);
                    }

                });
            }
            return true;
        }

        public static bool TryGetNodeData<T>(this INode node,out T data) where T:INodeData{
            if (node is ICustomizedNode CNode)
            {
                return CNode.TryGetNodeData<T>(out data);

            }

            if (INodeDataSystem.nodeInfo.ContainsKey(node))
            {
                var NodeData = node.GetOrCreateNodeDataCollection();
                if (NodeData.TryGetValue(typeof(T), out var ndata))
                {
                    data = (T)ndata;
                    return true;
                }
            }

            data = default;
            return false;

        }

        public static INodeData[] GetAllNodeData(this INode node)
        {
            if (node is ICustomizedNode CNode)
            {
                return CNode.GetAllNodeData();

            }

            var rs = new INodeData[0];
            if (INodeDataSystem.nodeInfo.ContainsKey(node))
            {
                var NodeData = node.GetOrCreateNodeDataCollection();
                rs = NodeData.Values.ToArray();
            }
            return rs;

        }

        public static bool TryGetFirstNodeDataInChildren<T>(this INode node, out T data) where T : INodeData {
            if (node is ICustomizedNode CNode)
            {
                return CNode.TryGetFirstNodeDataInChildren<T>(out data);

            }

            var kids = node.GetAllKids();
            foreach (var kid in kids) {
                if (kid.TryGetNodeData(out data)) {
                    return true;
                }
            }

            data =default;
            return false;
        }

        public static bool TryGetFirstNodeDataInParent<T>(this INode node, out T data) where T : INodeData
        {
            if (node is ICustomizedNode CNode)
            {
                return CNode.TryGetFirstNodeDataInParent(out data);
            }

            if (node.TryGetParentNode(out var parent))
            {
                if (parent.TryGetNodeData(out data))
                {
                    return true;
                }
                else {
                    return parent.TryGetFirstNodeDataInParent(out data);
                }
            }
          
            data = default;
            return false;
            
        }


        public static T GetOrCreateNodeData<T>(this INode node, T defaultNode) where T:INodeData{
            if (node is ICustomizedNode CNode)
            {
                return CNode.GetOrCreateNodeData<T>(defaultNode);
            }

            if (node.TryGetNodeData(out T data))
            {
                return data;
            }
            else {

                return (T)node.AddNodeData(defaultNode);
            }

        }
        
	    public static T GetOrCreateNodeData<T>(this INode node) where T:INodeData{
            if (node is ICustomizedNode CNode)
            {
                return CNode.GetOrCreateNodeData<T>();
            }

            if (node.TryGetNodeData<T>(out T data))
		    {
			    return data;
		    }
		    else {

			    var defaultNode = (T)Activator.CreateInstance(typeof(T));
			    return node.AddNodeData(defaultNode);
		    }

	    }

        public static bool TryGetGlobalNodeData<T>(this IGlobalNodeDataAccessor accessor, out T data) where T: INodeData{

            return INodeSystem.GlobalNode.TryGetNodeData<T>(out data);
        }

        public static INodeData AddGlobalNodeData<T>(this IGlobalNodeDataAccessor accessor, INodeData data) where T : INodeData
        {
            return INodeSystem.GlobalNode.AddNodeData(data);
        }

        public static INodeData AddGlobalNodeData(this IGlobalNodeDataAccessor accessor, INodeData data)
        {
            return INodeSystem.GlobalNode.AddNodeData(data);
        }



        public static bool RequestGlobalNodeData<T>(this IGlobalNodeDataAccessor requestor,System.Action<T> request) where T : INodeData {
            return INodeSystem.GlobalNode.RequestNodeData(request);
        }
    }
    public struct NodeInfo{
        public Dictionary<Type,INodeData> nodeData;
        public Dictionary<Type,List<Action<INodeData>>> requests;
    }

    public class NodeDataInfo{
        public INode Root;

        public static NodeDataInfo Create(){
            return new NodeDataInfo(){};
        }
    }
}