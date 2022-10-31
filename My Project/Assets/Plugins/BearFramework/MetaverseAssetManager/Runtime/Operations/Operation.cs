using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Bear
{
    public enum OperationStatus
    {
        Idle,
        Processing,
        Success,
        Failed
    }
    public class Operation : IEnumerator
    {
        public AssetLoader loader;

        public Action<Operation> completed;
        public OperationStatus status { get; set; } = OperationStatus.Idle;
        public float progress { get; protected set; }
        public bool isDone => status == OperationStatus.Failed || status == OperationStatus.Success;
        public string error { get; protected set; }
        public Task<Operation> Task
        {
            get
            {
                var tcs = new TaskCompletionSource<Operation>();
                completed += operation => { tcs.SetResult(this); };
                return tcs.Task;
            }
        }

        public bool MoveNext()
        {
            return !isDone;
        }

        public void Reset()
        {
        }

        public object Current => null;

        public virtual void Update()
        {
        }
        
	    public virtual void OnStart(){
	    	
	    }


        public void Cancel()
        {
            Finish("User Cancel.");
        }
        protected void Finish(string errorCode = null)
        {
            error = errorCode;
            status = string.IsNullOrEmpty(error) ? OperationStatus.Success : OperationStatus.Failed;
            progress = 1;
        }

	    public void Complete()
        {
            if (completed == null) return;

            var saved = completed;
            completed.Invoke(this);
            completed -= saved;
        }

        
    }
}