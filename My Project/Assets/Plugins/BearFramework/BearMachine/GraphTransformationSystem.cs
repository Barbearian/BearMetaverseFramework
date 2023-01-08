using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BearMachine{
	
	public interface IGraphTransformer{
		public Graph Transform(Graph graph);
	}
	
	public static class GraphTransformationSystem
	{
	    
	}
	
	public class NaiveEdgeTransformer:IGraphTransformer{
		public EdgeTransform[] transformations;
		public Graph Transform(Graph graph){
			foreach (var transformation in transformations)
			{
				switch (transformation.Operation)
				{
				case EEdgeTransform.Add:
					graph.Edges.Add(transformation.TargetEdge);
					break;
						
				case EEdgeTransform.Remove:
					graph.Edges.Remove(transformation.TargetEdge);
					break;
						
				default:
					break;
				}
				
			
			}
			return graph;
		}
	}
	
	public class ReplaceGraphTransformer:Graph,IGraphTransformer{
		public Graph Transform(Graph graph){
			return this;
		}
	}
}