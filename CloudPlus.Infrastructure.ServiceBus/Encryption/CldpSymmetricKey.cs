using MassTransit.Serialization;

namespace CloudPlus.Infrastructure.ServiceBus.Encryption
{
    public class CldpSymmetricKey : SymmetricKey
    {
        readonly byte[] _iv;
        readonly byte[] _key;

        public CldpSymmetricKey(byte[] key, byte[] iv)
        {
            _key = key;
            _iv = iv;
        }
        byte[] SymmetricKey.Key => _key;
        byte[] SymmetricKey.IV => _iv;
    }
}