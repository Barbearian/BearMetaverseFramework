using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear {
    public interface IGlobalMailSender:ISignalSender { 
    
    }

    public interface IGlobalMailRegister : ISignalSender { 
    
    }

    public static class IGlobalMailBoxHandler {
        public static void RegisterGlobalMailBox(this IGlobalMailRegister box, string address,ISignalReceiver receiver) {
            box.SendSignal(new RegisterSignal(address,receiver));
        }

        public static void SendGlobalMail(this IGlobalMailSender box, string address, ISignal mail)
        {
            box.SendSignal(new DeliverSignal(address, mail));
        }

        public static void SendGlobalMail(this IGlobalMailSender box, ISignal mail)
        {
            box.SendSignal(new DeliverSignal(mail.GetType().ToString(), mail));
        }

        public static void RegisterGlobalMailBox(this IGlobalMailRegister box, ISignalReceiver receiver)
        {
            box.RegisterGlobalMailBox(receiver.GetType().ToString(), receiver);
        }

        public static void RegisterGlobalMailBox<T>(this IGlobalMailRegister box, ISignalReceiver receiver)
        {
            box.RegisterGlobalMailBox(typeof(T).ToString(), receiver);
        }

        public static void RegisterGlobalMailBox<T>(this IGlobalMailRegister box, System.Action<ISignal> action)
        {
            box.RegisterGlobalMailBox(typeof(T).ToString(), new ActionSignalReceiver(action));
        }

        public static void DeRegisterGlobalMailBox(this IGlobalMailRegister box,string address)
        {
            box.SendSignal(new RegisterSignal(address, null) { 
                isRegister = false
            });
        }

        public static void DeRegisterGlobalMailBox<T>(this IGlobalMailRegister box)
        {
            box.SendSignal(new RegisterSignal(typeof(T).ToString(), null)
            {
                isRegister = false
            });
        }
    }
}