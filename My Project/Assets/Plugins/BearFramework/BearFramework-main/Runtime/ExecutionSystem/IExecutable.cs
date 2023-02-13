using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bear
{
    public interface IExecutable
    {
        public void Execute();
    }

    public interface ISignalSenderExSignal : IExecutableNodeSignal
    {
        public INode Receiver { get; set; }
    }
}