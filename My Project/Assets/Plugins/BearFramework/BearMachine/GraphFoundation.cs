using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BearMachine{
	using System;
	using System.Runtime.CompilerServices;
	using System.Security.Cryptography;
	public struct Node{
		public int index;
		public bool isFixed;
		
		public override bool Equals(object obj)
		{
			if(obj is Node nobj){
				return nobj.index == index && nobj.isFixed == isFixed;
			}
			return false;
		}

        public override int GetHashCode()
        {
			return HashCode.Combine(index,isFixed);
        }


    }
	
	public struct Edge{
		public Node Source;
		public Node Rel;
		public Node End;
		
		public override bool Equals(object obj)
		{
			if(obj is Edge eobj){
				return End.Equals(eobj.End) 
					&& Rel.Equals(eobj.Rel)
					&& Source.Equals(eobj.Source);
			}
			
			return false;
		}

        public override int GetHashCode()
		{
            return HashCode.Combine(Source, Rel,End);
        }
    }
	
	public class Graph{
		public List<Edge> Edges;
	}
	
	public class EdgeTransform{
		public Edge TargetEdge;
		public EEdgeTransform Operation;
	}
	
	public class GraphTransformation{
		public List<EdgeTransform> transformations;
	}
	
	public enum EEdgeTransform{
		Add,
		Remove,
	}
	
	public class NodeMap{
		public Dictionary<int,int> map;
		public bool MappingResult;
	}
	

	
	
}