using UnityEngine.Networking;

namespace Tapsell.Mediation.Adapter.Legacy.Utils
{
    /**
     * This is a fix for "Curl error 60: Cert verify failed"
     */
    public class CertificateWhore: CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}