namespace Bear {
    public interface IMoveSignal { }
    public struct MovementSignalFilter : ISignalFilter
    {
        public bool CanPass(INodeSignal signal = null)
        {
            return signal is not IMovementSignal;
        }
        public override bool Equals(object obj)
        {
            return GetType().Equals(obj.GetType());
        }

        public override int GetHashCode()
        {

            return GetType().GetHashCode();
        }
    }

    public interface IMovementSignal { }
}