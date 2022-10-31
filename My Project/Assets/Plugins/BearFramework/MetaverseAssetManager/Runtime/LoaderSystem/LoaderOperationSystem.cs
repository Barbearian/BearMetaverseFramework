using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    
    public static class LoaderOperationSystem 
    {
	    public static void ProcessOperation(this AssetLoader Loader,Operation operation){
	    	Loader.Processing.Add(operation);
	    }
	    
	    public static void StartOperation(this AssetLoader Loader,Operation operation){
	    	operation.loader = Loader;
	    	operation.status = OperationStatus.Processing;
	    	Loader.ProcessOperation(operation);
	    	
	    	operation.OnStart();
	    }
    }
}