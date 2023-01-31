namespace Bear
{
    public struct EnterAttackSignal : INodeSignal, IExecutable
    {
        public PlayAnimationNodeSignal signal;
        public INode target;
        public void Execute()
        {
            target.ReceiveNodeSignal(signal);
        }
    }

}