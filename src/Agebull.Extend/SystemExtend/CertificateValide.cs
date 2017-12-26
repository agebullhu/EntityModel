// 所在工程：GBoxtCommonService
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12
#if !NETSTANDARD2_0

#region

using System.IdentityModel.Selectors ;
using System.Security.Cryptography.X509Certificates ;

using Agebull.Common.Logging ;

#endregion

namespace Agebull.Common.Security.Certificate.X509
{
    /// <summary>
    ///   证书检验--无作用
    /// </summary>
    public class CertificateValide : X509CertificateValidator
    {
        /// <summary>
        ///   校验证书
        /// </summary>
        /// <param name="certificate"> 证书 </param>
        public override void Validate(X509Certificate2 certificate)
        {
            LogRecorder.Trace(certificate.Subject) ;
            LogRecorder.Trace(certificate.Thumbprint) ;
        }
    }
}

#endif
