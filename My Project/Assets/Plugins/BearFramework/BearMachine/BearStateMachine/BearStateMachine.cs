namespace BearMachine{
	public class BearStateMachine 
	{
		public Graph edges;
		public NaiveBearMachine nbm;
		public BearStateMachine(){
			edges = new Graph();
		}

		
		public void AddEdge(int index,int type){
			var edge = new Edge();
			edge.End.index = index;
			edge.Source.index = type;
			edges.Edges.Add(edge);
		}
		
		public void UpdateCurrentState(){
			var indexes = edges.GetMap(nbm);
			foreach (var index in indexes)
			{
				edges = edges.Transform(nbm,index);
			}
		}
		
	
	}
	
	
}