

namespace Bear {

    public class LinkNodeData : SignalHandlerNodeData, ILinkMaker, IOnAttachedToNode
    {
        public INode Target { get; set; }
        protected INode Source { get; set; }
        protected ArrayNodeSignalReceiver OutEdges { get; set; } = new ArrayNodeSignalReceiver();
        public virtual void Attached(INode node)
        {
            Source = node;

            var (linkKey, delinkKey) = GetKeys();
            var LinkAction = new ActionNodeSignalReceiver((x) => {
                if (x is ILinkTargetWielder lSignal) {
                    Link(lSignal.Target);
                }
            });
            LinkAction.AddTo(receivers);

            var DelinkAction = new ActionNodeSignalReceiver((x) => {
                if (x is ILinkTargetWielder lSignal)
                {
                    Delink(lSignal.Target);
                }
            });
            DelinkAction.AddTo(receivers);


            Source.RegisterSignalReceiver(linkKey,LinkAction, true);
            Source.RegisterSignalReceiver(delinkKey,DelinkAction, true);
        }

        public virtual (string, string) GetKeys() {
            return (typeof(ILinkSignal).ToString(), typeof(IDelinkSignal).ToString());
        }

        public virtual void Delink(INode node)
        {
            if (Target == null || node == null) {
                return;
            }

            if (Target.Equals(node)) { 
                Target= null;  
            }

            OutEdges.InhibitAll();
        }

        public virtual void Link(INode node)
        {
            Target = node;

            var translate = new ActionNodeSignalReceiver((x) =>
            {
                Target.ReceiveNodeSignal(x);
            });

            translate.AddTo(OutEdges);

            Source.RegisterSignalReceiver(this.GetType().ToString(), translate);

        }

        public override void Detached(INode node)
        {
            base.Detached(node);
            if(Target != null)
                Delink(Target);
        }

        protected static (string, string) GetKeys<T, K>() {
            return (typeof(T).ToString(),typeof(K).ToString());
        }
    }



    public interface ILinkTargetWielder {
        public INode Target { get; set; }

    }

    public interface ILinkMaker
    {
        public void Link(INode node);
        public void Delink(INode node);
    }

    public interface ILinkSignal: ILinkTargetWielder,INodeSignal
    {
    }

    public interface IDelinkSignal: ILinkTargetWielder, INodeSignal
    {
    }
}