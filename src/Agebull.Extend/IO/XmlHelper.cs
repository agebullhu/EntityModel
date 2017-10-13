// 所在工程：GBoxtCommonService
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:14

#region

using System.Text ;
using System.Xml ;
using System.Xml.Linq ;


#endregion

namespace Agebull.Common.Xml
{
    /// <summary>
    ///   XML 文件解析的操作包装
    /// </summary>
    public class XmlHelper
    {
        #region XML的辅助操作

        /// <summary>
        ///   移动读取器到一个位置
        /// </summary>
        /// <param name="xr"> 读取器 </param>
        /// <param name="parfriend"> </param>
        /// <param name="path"> 路径 </param>
        /// <returns> 成功或失败 </returns>
        public static bool MoveXmlTo(XmlReader xr , string parfriend , string[] path)
        {
            return xr.ReadToFollowing(parfriend) && MoveXmlTo(xr , path) ;
        }

        /// <summary>
        ///   移动读取器到一个位置
        /// </summary>
        /// <param name="xr"> 读取器 </param>
        /// <param name="path"> 路径 </param>
        /// <returns> 成功或失败 </returns>
        public static bool MoveXmlTo(XmlReader xr , string[] path)
        {
            foreach(string t in path)
            {
                if(!xr.ReadToFollowing(t))
                {
                    return false ;
                }
            }
            return true ;
        }

        /// <summary>
        ///   读取当前位位置的文本(已做TRIM处理)
        /// </summary>
        /// <param name="xr"> 读取器 </param>
        /// <returns> 文本 </returns>
        public static string ReadXmlText(XmlReader xr)
        {
            try
            {
                xr.Read() ;
                return xr.ReadContentAsString().Trim() ;
            }
            catch
            {
                return null ;
            }
        }

        /// <summary>
        ///   读取当前位位置的一个属性的值
        /// </summary>
        /// <param name="xr"> 读取器 </param>
        /// <param name="attName"> 属性名 </param>
        /// <returns> 属性值 </returns>
        public static string ReadXmlAttrib(XmlReader xr , string attName)
        {
            try
            {
                if(!xr.MoveToAttribute(attName))
                {
                    return null ;
                }
                if(!xr.ReadAttributeValue())
                {
                    return null ;
                }
                return xr.Value ;
            }
            catch
            {
                return null ;
            }
        }

        #endregion

#if !SILVERLIGHT
        /// <summary>
        ///   读取当前位位置的一个属性的值
        /// </summary>
        /// <param name="doc"> 读取器 </param>
        /// <param name="par"> 属性节点($SG$) </param>
        /// <param name="name"> 属性名($SG$) </param>
        /// <param name="value"> 属性值($SG$) </param>
        /// <returns> 属性值 </returns>
        public static XmlNode Write(XmlDocument doc , XmlNode par , string name , object value = null)
        {
            XmlNode sc = doc.CreateNode(XmlNodeType.Element , name , "") ;
            par.AppendChild(sc) ;
            if(value != null)
            {
                sc.InnerText = value.ToString() ;
            }
            return sc ;
        }

        /// <summary>
        ///   生成XML
        /// </summary>
        /// <param name="rootName"> </param>
        /// <param name="nodes"> </param>
        /// <returns> </returns>
        public static string BuildXml(string rootName , params string[] nodes)
        {
            XElement doc = new XElement(rootName);
            if (nodes != null && nodes.Length >= 2)
            {
                for (int i = 1; i < nodes.Length; i += 2)
                {
                    doc.SetAttributeValue(nodes[i - 1],nodes[i]);
                }
            }
            return doc.ToString();
        }
#endif

    }
}
