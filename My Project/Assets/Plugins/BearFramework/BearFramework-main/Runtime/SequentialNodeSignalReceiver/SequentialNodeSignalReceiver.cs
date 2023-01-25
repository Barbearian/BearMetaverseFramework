

namespace Bear
{
    public interface ISequentialNodeSignalReceiver : INodeSignalReceiver
    {
        public INodeSignalReceiver Output { get; set; }
    }

   
}