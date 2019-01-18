using System;
using System.Collections.Generic;
using MassTransit.Serialization;

namespace CloudPlus.Infrastructure.ServiceBus.Encryption
{
    public class CldpKeyProvider : ISymmetricKeyProvider
    {
        private static string _base64Key = "9qy68e8Z25+ybVafV4n9v2j4OKFwKho5FnENTb78Xco=";
        private static string _base64iv = "+99Iwhs+ognGiMwouFqYzQ==";
        readonly Dictionary<string, CldpSymmetricKey> _keys;

        public CldpKeyProvider()
        {
            _keys = new Dictionary<string, CldpSymmetricKey>
            {
                {
                    "default",
                    new CldpSymmetricKey(Convert.FromBase64String(_base64Key), Convert.FromBase64String(_base64iv))
                }
            };
        }
        public bool TryGetKey(string id, out SymmetricKey key)
        {
            var found = _keys.TryGetValue(id, out var symmetricKey);
            if (found)
            {
                key = symmetricKey;
                return true;
            }
            key = null;
            return false;
        }
    }
}