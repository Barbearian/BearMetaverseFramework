using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public interface INodeSignal
	{
	
	}


	
	public interface INodeSignalReceiver
	{
		public void Receive(INodeSignal signal);
		public bool IsActive{ get; set; }
	}

	public class ActionNodeSignalReceiver : INodeSignalReceiver
    {
		public Action<INodeSignal> action;
		public void Receive(INodeSignal signal) {
			try {
				action?.Invoke(signal);
			} catch (Exception e) {
				Debug.LogWarning(e);
                _isActive = false;
			}
		}

		public ActionNodeSignalReceiver(Action<INodeSignal> action)
		{
			this.action = action;
            _isActive = true;
		}

		public void SetActive(bool isActive) { _isActive = isActive; }
		
		bool _isActive = true;
        public bool IsActive { get => _isActive; set { _isActive = value; } }
    }

    public class ArrayNodeSignalReceiver : INodeSignalReceiver
    {
        bool _isActive = true;
        public bool IsActive { get => _isActive; set { _isActive = value; } }

		public List<INodeSignalReceiver> receivers = new();

        void INodeSignalReceiver.Receive(INodeSignal signal)
        {
			for (int i = receivers.Count-1; i >=0; i--)
			{
				var receiver = receivers[i];
				if (receiver.IsActive)
				{
					receiver.Receive(signal);
				}
				else {
					receivers.RemoveAt(i);
				}
			}
        }

    }

	public static class ArrayNodeSignalReceiverSystem {
		public static void AddTo(this INodeSignalReceiver receiver, ArrayNodeSignalReceiver receivers) { 
			receivers.receivers.Add(receiver);
		}

        public static void InhibitAll(this ArrayNodeSignalReceiver receivers)
        {
			foreach (var item in receivers.receivers)
			{
				item.IsActive = false;
			}
        }
    }

	
}