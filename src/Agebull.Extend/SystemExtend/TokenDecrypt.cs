// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12
#if !NETCOREAPP

#region

using System ;
using System.IO ;
using System.Security.Cryptography.X509Certificates ;
using System.Security.Cryptography.Xml ;
using System.Xml ;

using Agebull.Common.Security.Certificate.X509 ;
using Agebull.Common.Xml ;

#endregion

namespace Agebull.Common.Security
{
    /// <summary>
    ///   XML解密
    /// </summary>
    public class XmlDecrypt : XmlHelper
    {
        /// <summary>
        ///   消息解密,以得到明文的内容
        /// </summary>
        /// <param name="xml"> 已加密Token主体XML </param>
        /// <returns> 解密后的Token主体XML </returns>
        public static string Decrypt(string xml)
        {
            StringReader sr = new StringReader(xml) ;
            XmlReader xr = XmlReader.Create(sr) ;
            xr.ReadToDescendant("trust:RequestSecurityTokenResponse") ;
            xr.ReadToFollowing("trust:RequestedSecurityToken") ;
            xr.ReadToDescendant("xenc:EncryptedData") ;
            xr.ReadToDescendant("KeyInfo") ;
            xr.ReadToDescendant("e:EncryptedKey") ;
            xr.ReadToDescendant("KeyInfo") ;
            xr.ReadToDescendant("o:SecurityTokenReference") ;
            xr.ReadToDescendant("X509Data") ;
            xr.ReadToDescendant("X509IssuerSerial") ;
            xr.ReadToDescendant("X509IssuerName") ;
            xr.Read() ;
            string cername = xr.ReadContentAsString().Trim() ;
            xr.ReadToNextSibling("X509SerialNumber") ;
            xr.Read() ;
            string cersn = xr.ReadContentAsString().Trim() ;
            xr.Close() ;
            XmlDocument doc = new XmlDocument() ;
            doc.InnerXml = xml ;
            XmlNamespaceManager xm = new XmlNamespaceManager(doc.NameTable) ;
            xm.AddNamespace("trust" , "http://docs.oasis-open.org/ws-sx/ws-trust/200512") ;
            xm.AddNamespace("wsu" , "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd") ;
            xm.AddNamespace("xenc" , "http://www.w3.org/2001/04/xmlenc#") ;
            xm.AddNamespace("e" , "http://www.w3.org/2001/04/xmlenc#") ;
            xm.AddNamespace("e" , "http://www.w3.org/2001/04/xmlenc#") ;
            xm.AddNamespace("" , "http://www.w3.org/2000/09/xmldsig#") ;
            XmlNode old = doc.SelectSingleNode("//trust:RequestSecurityTokenResponse" , xm) ;
            if(old == null)
            {
                return null ;
            }
            old = old.SelectSingleNode("//trust:RequestedSecurityToken" , xm) ;
            if(old == null)
            {
                return null ;
            }
            old = old.SelectSingleNode("//xenc:EncryptedData" , xm) ;
            if(old != null)
            {
                foreach(XmlNode n in old.ChildNodes)
                {
                    if(n.LocalName != "KeyInfo")
                    {
                        continue ;
                    }
                    old = n ;
                    break ;
                }
            }
            if(old == null)
            {
                return null ;
            }
            old = old.SelectSingleNode("//e:EncryptedKey" , xm) ;
            if(old == null)
            {
                return null ;
            }
            foreach(XmlNode n in old.ChildNodes)
            {
                if(n.LocalName != "KeyInfo")
                {
                    continue ;
                }
                old = n ;
                break ;
            }
            old.InnerXml = "<KeyName>rsaKey</KeyName>" ;
            DecryptDocument(doc , cername , cersn , "rsaKey") ;
            return doc.InnerXml ;
        }

        /// <summary>
        ///   对XmlDocument执行解密
        /// </summary>
        /// <param name="doc"> XmlDocument对象 </param>
        /// <param name="cername"> 证书名称 </param>
        /// <param name="cersn"> 证书的序列号(已被Asn1IntegerConverter转换过的) </param>
        /// <param name="keyname"> 加密的标签名 </param>
        public static void DecryptDocument(XmlDocument doc , string cername , string cersn , string keyname)
        {
            X509Certificate2 x = CertificateHelper.GetCertificate(StoreName.My , StoreLocation.LocalMachine , cername) ;
            string thisSN = Asn1IntegerConverter.Asn1IntegerToDecimalString(x.GetSerialNumber()) ;
            if(thisSN != cersn)
            {
                throw new Exception("无法分析文本") ;
            }
            EncryptedXml exml = new EncryptedXml(doc) ;
            exml.AddKeyNameMapping(keyname , x.PrivateKey) ;
            exml.DecryptDocument() ;
        }
    }
}

#endif
