using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear
{
    public class MailBox : IBearService,ISignalReceiver
    {
        public Dictionary<string, ISignalReceiver> mailOffice = new Dictionary<string, ISignalReceiver>();
        bool _isActive = true;
        public bool IsActive { get => _isActive; set => _isActive = value; }

        public void Init(IBearFramework framewrok)
        {
            //throw new System.NotImplementedException();
        }

        public void RegisterMailBox(string addrees, ISignalReceiver receiver)
        {
            if (mailOffice.TryGetValue(addrees, out var value))
            {
                if (value is MailBuffer buffer)
                {

                    buffer.Deliver(receiver);
                }
            }

            mailOffice[addrees] = receiver;

        }

        public void Deliver(string address, ISignal mail)
        {
            if (mailOffice.TryGetValue(address, out var receiver) && receiver != null)
            {

                receiver.ReceiveSignal(mail);
            }
            else
            {
                var mailbox = new MailBuffer();
                RegisterMailBox(address, mailbox);
                mailbox.ReceiveSignal(mail);
            }
        }

        public void ReceiveSignal(ISignal signal)
        {
            if (signal is DeliverSignal dsignal) {
                Deliver(dsignal.Address, dsignal.mail);
            } else if (signal is RegisterSignal rsignal) {
                if (rsignal.isRegister)
                {
                    RegisterMailBox(rsignal.Address,rsignal.receiver);
                }
                else {
                    mailOffice[rsignal.Address] = null;
                }
            }
        }
    }

    public class MailBuffer : ISignalReceiver
    {
        public List<ISignal> mails = new List<ISignal>();

        public bool IsActive { get => true; set { } }

        public void ReceiveSignal(ISignal mail)
        {
            mails.Add(mail);
        }

        public void Deliver(ISignalReceiver receiver)
        {
            foreach (var item in mails)
            {
                receiver.ReceiveSignal(item);
            }
        }
    }

    public class DeliverSignal:ISignal {
        public string Address;
        public ISignal mail;

        public DeliverSignal(string Address, ISignal mail) {
            this.Address = Address;
            this.mail = mail;
        }
    }

    public class RegisterSignal :ISignal{
        public bool isRegister;
        public string Address;
        public ISignalReceiver receiver;

        public RegisterSignal(string address, ISignalReceiver receiver, bool isRegister = true) {
            Address = address;
            this.receiver = receiver;
            this.isRegister = isRegister;
        }
    }

    public static class ReceiveMailHandler {
        public static void ReceiveMail(this ISignalReceiver receiver, string address, ISignal signal) {
            receiver.ReceiveSignal(new DeliverSignal(address,signal));
        }

        public static void ReceiveMail(this ISignalReceiver receiver, ISignal signal)
        {
            receiver.ReceiveSignal(new DeliverSignal(signal.GetType().ToString(), signal));
        }
    }

    
}