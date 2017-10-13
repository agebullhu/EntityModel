// 所在工程：GBoxtCommonService
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12
#if !SILVERLIGHT

#region

using System ;
using System.IO ;
using System.Security.Cryptography.X509Certificates ;

#endregion

namespace Agebull.Common.Security.Certificate.X509
{
    /// <summary>
    ///   查找返回X509证书
    /// </summary>
    public class CertificateHelper
    {
        /// <summary>
        ///   返回X509证书
        /// </summary>
        /// <returns> </returns>
        public static X509Certificate2 GetCertificate(StoreName name , StoreLocation location , string certInfo)
        {
            certInfo = certInfo.Replace(" " , "").ToLower() ;
            X509Store store = new X509Store(name , location) ;
            X509Certificate2Collection certificates = null ;
            store.Open(OpenFlags.ReadOnly) ;
            try
            {
                X509Certificate2 result = null ;
                //
                // 每次调用store.Certificates属性将返回一个新的集合
                //
                certificates = store.Certificates ;
                foreach (X509Certificate2 cert in certificates)
                {
                    if (cert.SubjectName.Name == null)
                    {
                        continue;
                    }
                    string cer_name = cert.SubjectName.Name.Replace(" " , "").ToLower() ;
                    // 使用证书名字作为唯一key检索
                    // 或者使用证书序列号检索
                    //string serialNum = Asn1IntegerConverter.Asn1IntegerToDecimalString(cert.GetSerialNumber());
                    if(cer_name == certInfo)
                    {
                        if(result != null)
                        {
                            throw new ApplicationException(string.Format("There is more than one certificate found for subject Name {0}" , certInfo)) ;
                        }
                        result = new X509Certificate2(cert) ;
                    }
                }
                if(result == null)
                {
                    throw new ApplicationException(string.Format("No certificate was found for subject Name {0}" , certInfo)) ;
                }
                return result ;
            }
            finally
            {
                if(certificates != null)
                {
                    foreach (X509Certificate2 cert in certificates)
                    {
                        cert.Reset() ;
                    }
                }
                store.Close() ;
            }
        }

        /// <summary>
        ///   引入一个X509证书
        /// </summary>
        /// <param name="certfilename"> 证书路径地址 </param>
        /// <param name="password"> 引入证书密码 </param>
        /// <returns> 证书 </returns>
        public static X509Certificate2 ImportCertificate(string certfilename , string password)
        {
            X509Certificate2 x509Certificate2 = new X509Certificate2() ;
            try
            {
                x509Certificate2.Import(certfilename , "agebull" , X509KeyStorageFlags.DefaultKeySet) ;
                return x509Certificate2 ;
            }
            catch(Exception ex)
            {
                throw new ApplicationException(ex.Message) ;
            }
        }

        /// <summary>
        ///   引入目录下的有相同序列号的X509证书
        /// </summary>
        /// <param name="serialnum"> 经过处理的序列号 </param>
        /// <param name="path"> 存放导出的证书的目录 </param>
        /// <returns> 证书 </returns>
        public static X509Certificate2 ImportCertificates(string path , string serialnum)
        {
            X509Certificate2 x509Certificate2 = new X509Certificate2() ;

            // 获取证书文件夹下所有证书
            DirectoryInfo dir = new DirectoryInfo(path) ;
            FileInfo[] files = dir.GetFiles() ;

            try
            {
                // 遍历所有证书的序列号，返回符合条件的证书
                foreach (FileInfo fileInfo in files)
                {
                    x509Certificate2.Import(fileInfo.FullName , "agebull" , X509KeyStorageFlags.DefaultKeySet) ;
                    if(serialnum == Asn1IntegerConverter.Asn1IntegerToDecimalString(x509Certificate2.GetSerialNumber()))
                    {
                        return x509Certificate2 ;
                    }
                    // 重置
                    x509Certificate2.Reset() ;
                }
                throw new ApplicationException("找不到匹配的证书！") ;
            }
            catch(Exception ex)
            {
                throw new ApplicationException(ex.Message) ;
            }
        }

        /// <summary>
        ///   引入X509证书
        /// </summary>
        /// <param name="rawData"> 证书原始字节组 </param>
        /// <returns> 证书 </returns>
        public static X509Certificate2 ImportCertificate(byte[] rawData)
        {
            X509Certificate2 x509Certificate2 = new X509Certificate2() ;
            try
            {
                x509Certificate2.Import(rawData) ;
                return x509Certificate2 ;
            }
            catch(Exception ex)
            {
                throw new ApplicationException(ex.Message) ;
            }
        }
    }
}

#endif
