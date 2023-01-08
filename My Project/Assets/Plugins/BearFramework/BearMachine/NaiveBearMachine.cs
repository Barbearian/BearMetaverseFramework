using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BearMachine{
	public class NaiveBearMachine
	{
		public List<Graph> MappedStates = new List<Graph>();
		public List<IGraphTransformer> Transformations = new List<IGraphTransformer>();
	}
	
	public static class NaiveBearMachineSystem{
		public static int[] GetMap(this Graph MyState, NaiveBearMachine machine){
			List<int> rs = new List<int>();
			for (int i = 0; i < machine.MappedStates.Count; i++) {
				var graph = machine.MappedStates[i];
				if(MyState.Contains(graph)){
					rs.Add(i);
				}
			}
			return rs.ToArray();
		}
		
		public static bool Contains(this Graph graph, Graph target){
			foreach (var edge in target.Edges)
			{
				if(!graph.Edges.Contains(edge)){
					return false;	
				}
			}
			
			return true;
		}
		
		public static Graph Transform(this Graph graph,NaiveBearMachine BearMachine,int index){
			return BearMachine.Transformations[index].Transform(graph);
		}
		
		public static void AddNaiveEdge(this Graph graph,int end, int rel = 0, int source = 0){
			graph.Edges.Add(new Edge(){
				End = new Node(){
					index = end,
					isFixed = false
				},
				Rel = new Node(){
					index = rel,
					isFixed = false
				},
				Source = new Node(){
					index = source,
					isFixed = false
				}
				
			});
		}
	
	}
	
	
}