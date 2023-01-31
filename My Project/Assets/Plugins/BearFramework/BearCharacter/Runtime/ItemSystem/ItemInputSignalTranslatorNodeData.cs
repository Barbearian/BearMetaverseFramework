namespace Bear
{
    public class ItemInputSignalTranslatorNodeData : SignalHandlerNodeData, IOnAttachedToNode
    {
        public ISignalTranslator translator { get; set; } = new TypeSignalTranslator();
        private ArrayNodeSignalReceiver inputReceiver { get; set; } = new ArrayNodeSignalReceiver();
        public void Attached(INode node)
        {
            node.RegisterSignalReceiver<OnEquippedSignal>((x) => {
                Link(x.Target);
            }, true).AddTo(receivers);

            node.RegisterSignalReceiver<OnUnequippedSignal>((x) => {
                Delink(x.Target);
            }, true).AddTo(receivers);


        }

        public void Link(INode node) {
            node.RegisterSignalReceiver<IPlayerToItemNodeSignal>((x) => {
                if (translator.TryTranslate(x,out var rs)) {
                    node.ReceiveNodeSignal(rs);
                }
            }, true).AddTo(inputReceiver);
        }

        public void Delink(INode node)
        {
            inputReceiver.InhibitAll();
        }
    }

    public interface IPlayerToItemNodeSignal : INodeSignal { }
}