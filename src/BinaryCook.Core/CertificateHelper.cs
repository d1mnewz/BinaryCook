using System.Security.Cryptography.X509Certificates;
using BinaryCook.Core.Code;

namespace BinaryCook.Core
{
    public class CertificateHelper
    {
        private readonly StoreName _store;
        private readonly StoreLocation _location;

        public CertificateHelper(StoreName store, StoreLocation location)
        {
            _store = store;
            _location = location;
        }

        public X509Certificate2 GetByType(X509FindType type, string value, bool validOnly = false)
        {
            var store = new X509Store(_store, _location);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var certificateCollection = store.Certificates.Find(type, value, validOnly);

                Ensure.That(certificateCollection.Count > 0, "Certificate not installed in the store");
                return certificateCollection[0];
            }
            finally
            {
                store.Close();
            }
        }
    }
}