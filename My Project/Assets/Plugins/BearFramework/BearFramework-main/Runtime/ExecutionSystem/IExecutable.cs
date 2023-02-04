using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bear
{
    public interface IExecutable
    {
        public void Execute();
    }

    public class SignalSenderExSignal : IExecutableNodeSignal
    {
        public INodeSignal signal;
        public INode receiver;
        public void Execute()
        {
            receiver.ReceiveNodeSignal(signal);
        }
    }
}