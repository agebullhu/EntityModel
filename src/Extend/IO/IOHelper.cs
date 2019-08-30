// 所在工程：Agebull.EntityModel
// 整理用户：bull2
// 建立时间：2012-08-13 5:35
// 整理时间：2012-08-30 3:12

#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;

#if WPF
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Cache;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Resources;
#endif
#endregion

namespace Agebull.Common
{
    /// <summary>
    ///   文件或文件夹操作帮助类
    /// </summary>
    public static class IOHelper
    {
        #region 保存与载入
        /// <summary>
        ///   删除或清除一个目录下的所有文件和目录
        /// </summary>
        /// <param name="directory"> </param>
        /// <param name="delete"> 清除目录下的内容而不删除目录 </param>
        public static void DeleteDirectory(string directory, bool delete = true)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }
            var ds = Directory.GetDirectories(directory);
            foreach (var d in ds)
            {
                DeleteDirectory(d);
            }
            var fs = Directory.GetFiles(directory);
            foreach (var f in fs)
            {
                File.Delete(f);
            }
            if (delete)
            {
                Directory.Delete(directory);
            }
        }

        /// <summary>
        ///   检查一个路径是否存在,不存在则建立之
        /// </summary>
        /// <param name="root"> 根 </param>
        /// <param name="floders"> 路径,不可以用\的组合 </param>
        /// <returns> 组合成的路径 </returns>
        public static string CheckPath(string root, params string[] floders)
        {
            if (root == null)
            {
                return null;
            }
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            if (floders == null || floders.Length == 0)
            {
                return root;
            }
            var dirs = new List<string>();
            foreach (var floder in floders)
            {
                dirs.AddRange(floder.Split('\\').Where(p => !string.IsNullOrWhiteSpace(p)).Select(s => s.Trim()));
            }
            foreach (var p in dirs)
            {
                root = Path.Combine(root, p);
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
            }
            return root;
        }
        /// <summary>
        ///   检查一个路径是否存在,不存在则建立之
        /// </summary>
        /// <param name="path"> 目录 </param>
        /// <returns> 组合成的路径 </returns>
        public static string CheckPaths(string path)
        {
            if (path == null)
            {
                return null;
            }
            if (Directory.Exists(path))
            {
                return path;
            }
            var root = Path.GetPathRoot(path);
            var folders = path.Split(new char[] {'\\', '/'},StringSplitOptions.RemoveEmptyEntries).Skip(1);
            foreach (var folder in folders)
            {
                root = Path.Combine(root, folder);
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
            }
            return root;
        }

        /// <summary>
        ///   检查一个路径是否存在,不存在则建立之
        /// </summary>
        /// <param name="root"> 根 </param>
        /// <param name="floder"> 路径,可以用\的组合 </param>
        /// <returns> 组合成的路径 </returns>
        public static string CheckPaths(string root, string floder)
        {
            if (root == null)
            {
                return null;
            }
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            if (floder == null)
            {
                return root;
            }

            foreach (var p in floder.Split('\\', '/'))
            {
                root = Path.Combine(root, p);
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
            }
            return root;
        }

        /// <summary>
        ///   写文件
        /// </summary>
        /// <param name="path"> 路径(如果不存在会被创建 </param>
        /// <param name="name"> 文件名 </param>
        /// <param name="txt"> 要写入的文本 </param>
        /// <param name="alwaysWrite"> 是否总是写入,如果先否并且文件存在将不修改文件 </param>
        public static void WriteFile(string path, string name, string txt, bool alwaysWrite)
        {
            path = CheckPath(path);
            var fn = Path.Combine(path, name);
            var exists = File.Exists(fn);
            if (!alwaysWrite && exists)
            {
                return; //不能写
            }
            if (string.IsNullOrWhiteSpace(txt))
            {
                File.Delete(fn);
            }
            else
            {
                File.WriteAllText(fn, txt, Encoding.UTF8);
            }
        }

        /// <summary>
        ///   写文件
        /// </summary>
        /// <param name="fullName"> 路径(如果不存在会被创建 </param>
        /// <param name="txt"> 要写入的文本 </param>
        /// <param name="alwaysWrite"> 是否总是写入,如果先否并且文件存在将不修改文件 </param>
        public static void WriteFile(string fullName, string txt, bool alwaysWrite)
        {
            CheckPath(Path.GetDirectoryName(fullName));
            var exists = File.Exists(fullName);
            if (!alwaysWrite && exists)
            {
                return; //不能写
            }
            if (string.IsNullOrWhiteSpace(txt))
            {
                return; //不能写
            }
            File.WriteAllText(fullName, txt, Encoding.UTF8);
        }

        /// <summary>
        ///   保存对象
        /// </summary>
        /// <typeparam name="T"> 对象类型 </typeparam>
        /// <param name="t"> 对象 </param>
        /// <param name="fullName"> 文件名 </param>
        public static void Save<T>(T t, string fullName)
        {
            CheckPath(Path.GetDirectoryName(fullName));
            var writer = new FileStream(fullName, FileMode.Create);
            var ser = new BinaryFormatter();
            ser.Serialize(writer, t);
            writer.Close();
        }

        /// <summary>
        ///   载入对象
        /// </summary>
        /// <typeparam name="T"> 对象类型 </typeparam>
        /// <param name="fileName"> 文件名 </param>
        /// <returns> 对象 </returns>
        public static T TryLoad<T>(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return default(T);
            }
            var t = default(T);
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var ser = new BinaryFormatter();
                try
                {
                    t = (T)ser.Deserialize(fs);
                }
                catch
                {
                    return t;
                }
            }
            return t;
        }
        #endregion

        #region XML序列化

        /// <summary>
        ///   序列化到XML
        /// </summary>
        /// <param name="args"> </param>
        /// <returns> </returns>
        public static string XMLSerializer(object args)
        {
            if (args == null)
            {
                return null;
            }
            using (var ms = new MemoryStream())
            {
                var ds = new DataContractSerializer(args.GetType());
                ds.WriteObject(ms, args);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                var sr = new StreamReader(ms);
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        ///   序列化到XML
        /// </summary>
        /// <param name="args"> </param>
        /// <returns> </returns>
        public static string XMLSerializer<T>(T args)
        {
            if (Equals(args,default(T)))
                return null;
            using (var ms = new MemoryStream())
            {
                var ds = new DataContractSerializer(typeof(T));
                ds.WriteObject(ms, args);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                var sr = new StreamReader(ms);
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        ///   序列化到XML
        /// </summary>
        /// <param name="ms"> </param>
        /// <param name="args"> </param>
        /// <returns> </returns>
        public static bool XMLSerializer<T>(Stream ms, T args)
        {
            if (Equals(args, default(T)))
                return false;
            ms.SetLength(1);
            var ds = new DataContractSerializer(typeof(T));
            ds.WriteObject(ms, args);
            ms.Flush();
            return true;
        }

        /// <summary>
        ///   从XML反序列化
        /// </summary>
        /// <param name="ms"> </param>
        /// <returns> T </returns>
        public static T XMLDeSerializer<T>(Stream ms)
        {
            if (ms == null || ms.Length == 0)
                return default(T);
            var ds = new DataContractSerializer(typeof(T));
            return (T)ds.ReadObject(ms);
        }

        /// <summary>
        ///   从XML反序列化
        /// </summary>
        /// <param name="args"> </param>
        /// <returns> </returns>
        public static T XMLDeSerializer<T>(string args)
        {
            if (string.IsNullOrWhiteSpace(args) || args[0] != '<')
            {
                return default(T);
            }
            byte[] buffers;
            long len;
            using (var ms = new MemoryStream())
            {
                var sw = new StreamWriter(ms);
                sw.Write(args);
                sw.Flush();
                len = ms.Position;
                buffers = ms.GetBuffer();
            }
            using (var reader = XmlDictionaryReader.CreateTextReader(buffers, 0, (int)len, new XmlDictionaryReaderQuotas()))
            {
                var ds = new DataContractSerializer(typeof(T));
                var re = (T)ds.ReadObject(reader, false);
                return re;
            }
        }

        #endregion

        #region 读写扩展

        /// <summary>
        ///   读取一个文件的二进制数据
        /// </summary>
        /// <param name="filename"> </param>
        /// <returns> </returns>
        public static byte[] ReadBinary(string filename)
        {
            if (!File.Exists(filename))
            {
                return null;
            }
            var fs = new FileStream(filename, FileMode.Open);
            var buffer = new byte[(int)fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();
            return buffer;
        }

        /// <summary>
        ///   读取一个文件的文本数据
        /// </summary>
        /// <param name="filename"> </param>
        /// <returns> </returns>
        public static string ReadString(string filename)
        {
            return !File.Exists(filename)
                ? null
                : File.ReadAllText(filename, GetEncoding(filename));
        }
#if WINFORM
        /// <summary>
        ///     保存对应图像
        /// </summary>
        /// <param name="img">图像</param>
        /// <param name="file">文件全名(包括路径)</param>
        public static string SaveImage(Image img, string file)
        {
            if (img == null)
            {
                return null;
            }
            byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                buffer = new byte[ms.Length];
                ms.Read(buffer, 0, (int)ms.Length);
            }
            File.WriteAllBytes(file, buffer);
            return file;
        }

        /// <summary>
        ///     载入对应的图像
        /// </summary>
        /// <param name="file">文件全名(包括路径)</param>
        /// <returns>读出的图像(可能为空)</returns>
        public static Bitmap LoadImage(string file)
        {
            if (!File.Exists(file))
            {
                return null;
            }
            try
            {
                byte[] buffer = File.ReadAllBytes(file);
                MemoryStream ms = new MemoryStream(buffer);
                return Image.FromStream(ms) as Bitmap;
            }
            catch
            {
                return null;
            }
        }
#endif
        #endregion

        #region 文件名相关

        /// <summary>
        /// 将名称合并为合乎要求的文件名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string ToFileName(params string[] name)
        {
            if (name == null
                || name.Length == 0)
                return null;
            var sb = new StringBuilder();
            foreach (var n in name.Where(n => !string.IsNullOrWhiteSpace(n)))
            {
                if (sb.Length > 0)
                {
                    sb.Append("_");
                }
                sb.AppendFormat("{0}", n.Trim());
            }
            sb.Replace('\\', '_').Replace('(', '_').Replace(')', '_');
            return sb.ToString();
        }
        /// <summary>
        ///   得到一个路径下所有文件名
        /// </summary>
        /// <param name="path"> </param>
        /// <param name="ext"> </param>
        /// <returns> </returns>
        public static List<string> GetAllField(string path, string ext)
        {
            var fs = new List<string>();
            if (ext != null && ext.IndexOf("*", StringComparison.Ordinal) < 0 && ext.IndexOf("?", StringComparison.Ordinal) < 0)
            {
                ext = $"*.{ext}";
            }
            GetAllFiles(fs, path, ext);
            return fs.Distinct().ToList();
        }
        /// <summary>
        ///   得到一个路径下所有文件名
        /// </summary>
        /// <param name="path"> </param>
        /// <param name="ext"> </param>
        /// <returns> </returns>
        public static List<string> GetAllFiles(string path, string ext)
        {
            var fs = new List<string>();
            if (ext != null && ext.IndexOf("*", StringComparison.Ordinal) < 0 && ext.IndexOf("?", StringComparison.Ordinal) < 0)
            {
                ext = $"*.{ext}";
            }
            GetAllFiles(fs, path, ext);
            return fs.Distinct().ToList();
        }

        /// <summary>
        ///   得到一个路径下所有文件名
        /// </summary>
        /// <param name="fields"> </param>
        /// <param name="path"> </param>
        /// <param name="ext"> </param>
        /// <returns> </returns>
        private static void GetAllFiles(List<string> fields, string path, string ext)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            fields.AddRange(Directory.GetFiles(path, ext, SearchOption.TopDirectoryOnly));
            foreach (var ch in Directory.GetDirectories(path))
            {
                GetAllFiles(fields, ch, ext);
            }
        }

        /// <summary>
        ///   路径复制
        /// </summary>
        /// <param name="srcPath"> 源路径 </param>
        /// <param name="destPath"> 目的路径 </param>
        /// <param name="ext"> 包括的扩展名 </param>
        /// <param name="child"> 是否包括子文件 </param>
        /// <param name="replace"> 是否替换已存在的文件 </param>
        /// <param name="excludes"> 排除列表 </param>
        public static void CopyPath(string srcPath, string destPath, string ext = "*.*", bool child = true, bool replace = false, params string[] excludes)
        {
            if (!Directory.Exists(srcPath))
            {
                return;
            }
            CheckPath(destPath);
            foreach (var ch in Directory.GetFiles(srcPath, ext))
            {
                if (excludes != null && excludes.Contains(Path.GetExtension(ch), StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }
                var ch2 = Path.Combine(destPath, Path.GetFileName(ch));
                if (replace || !File.Exists(ch2))
                {
                    File.Copy(ch, ch2, true);
                }
            }
            if (!child)
            {
                return;
            }
            foreach (var ch in Directory.GetDirectories(srcPath))
            {
                if (excludes != null && excludes.Contains(Path.GetFileName(ch), StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }
                CopyPath(ch, Path.Combine(destPath, Path.GetFileName(ch) ?? throw new InvalidOperationException()), ext, true, replace, excludes);
            }
        }

        #endregion

        #region 文本文件编码来自网友

        /// <summary>
        ///   取得一个文本文件的编码方式。如果无法在文件头部找到有效的前导符，Encoding.Default将被返回。
        /// </summary>
        /// <param name="fileName"> 文件名。 </param>
        /// <returns> </returns>
        public static Encoding GetEncoding(string fileName)
        {
            return GetEncoding(fileName, Encoding.Default);
        }

        /// <summary>
        ///   取得一个文本文件流的编码方式。
        /// </summary>
        /// <param name="stream"> 文本文件流。 </param>
        /// <returns> </returns>
        public static Encoding GetEncoding(Stream stream)
        {
            return GetEncoding(stream, Encoding.Default);
        }

        /// <summary>
        ///   取得一个文本文件的编码方式。
        /// </summary>
        /// <param name="fileName"> 文件名。 </param>
        /// <param name="defaultEncoding"> 默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。 </param>
        /// <returns> </returns>
        public static Encoding GetEncoding(string fileName, Encoding defaultEncoding)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            var targetEncoding = GetEncoding(fs, defaultEncoding);
            fs.Close();
            return targetEncoding;
        }

        /// <summary>
        ///   取得一个文本文件流的编码方式。
        /// </summary>
        /// <param name="stream"> 文本文件流。 </param>
        /// <param name="defaultEncoding"> 默认编码方式。当该方法无法从文件的头部取得有效的前导符时，将返回该编码方式。 </param>
        /// <returns> </returns>
        public static Encoding GetEncoding(Stream stream, Encoding defaultEncoding)
        {
            var targetEncoding = defaultEncoding;
            if (stream == null || stream.Length < 2)
                return targetEncoding;
            //保存文件流的前4个字节
            byte byte3 = 0;
            //保存当前Seek位置
            var origPos = stream.Seek(0, SeekOrigin.Begin);
            stream.Seek(0, SeekOrigin.Begin);

            var nByte = stream.ReadByte();
            var byte1 = Convert.ToByte(nByte);
            var byte2 = Convert.ToByte(stream.ReadByte());
            if (stream.Length >= 3)
            {
                byte3 = Convert.ToByte(stream.ReadByte());
            }
            if (stream.Length >= 4)
            {
                // ReSharper disable ReturnValueOfPureMethodIsNotUsed
                Convert.ToByte(stream.ReadByte());
                // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            }
            //根据文件流的前4个字节判断Encoding
            //Unicode {0xFF, 0xFE};
            //BE-Unicode {0xFE, 0xFF};
            //UTF8 = {0xEF, 0xBB, 0xBF};
            if (byte1 == 0xFE && byte2 == 0xFF) //UnicodeBe
            {
                targetEncoding = Encoding.BigEndianUnicode;
            }
            if (byte1 == 0xFF && byte2 == 0xFE && byte3 != 0xFF) //Unicode
            {
                targetEncoding = Encoding.Unicode;
            }
            if (byte1 == 0xEF && byte2 == 0xBB && byte3 == 0xBF) //UTF8
            {
                targetEncoding = Encoding.UTF8;
            }
            //恢复Seek位置       
            stream.Seek(origPos, SeekOrigin.Begin);
            return targetEncoding;
        }

        #endregion

        #region WPF资源文件
#if WPF
        /// <summary>
        ///  初始化全局命令
        /// </summary>
        public static void MergedDictionaries(string name)
        {
            Uri uri = new Uri(string.Format("/Agebull.Common.Client.Windows.Program;component/Style/{0}.xaml", name), UriKind.Relative);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            // ReSharper disable PossibleNullReferenceException
            if (info == null)
            {
                return;
            }
            XmlReader xmlReader = XmlReader.Create(info.Stream);
            Application.Current.Resources.MergedDictionaries.Add(XamlReader.Load(xmlReader) as ResourceDictionary);
        }
        /// <summary>
        /// 载入图像(无网络错误异常)
        /// </summary>
        /// <param name="url"></param>
        /// <returns>网络正常则为你需要的图片,否则为空图片</returns>
        public static BitmapImage LoadImage(string url)
        {
            try
            {
                return new BitmapImage(new Uri(url), new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable));
            }
            catch (Exception)
            {
                return null ;
            }
        }
        /// <summary>
        /// 载入图像(无网络错误异常)
        /// </summary>
        /// <param name="url"></param>
        /// <returns>网络正常则为你需要的图片,否则为空图片</returns>
        public static BitmapImage LoadImage(Uri url)
        {
            try
            {
                return new BitmapImage(url, new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable));
            }
            catch (Exception)
            {
                return null;
            }
        }
#endif
        #endregion

        #region 取目录大小信息

        /// <summary>
        /// Linux下取目录大小信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static  DiskInfo FolderDiskInfo(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new DiskInfo();
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) )
            {
                return LinuxFolderDiskInfo(path);
            }
            else
            {
                return WindowsFolderDiskInfo(path);
            }
        }

        /// <summary>
        /// Linux下取目录大小信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static DiskInfo WindowsFolderDiskInfo(string path)
        {
            var info = new DiskInfo();

            var str_HardDiskName = path[0] + ":\\";

            var drive = System.IO.DriveInfo.GetDrives().FirstOrDefault(p=> path.IndexOf(p.Name,StringComparison.OrdinalIgnoreCase)==0);
            if(drive != null)
            {
                info.TotalSize = drive.TotalSize / (1024 * 1024);
                info.AvailableSize = drive.AvailableFreeSpace / (1024 * 1024);
                info.UsedSize = info.TotalSize - info.AvailableSize;
                info.Use = info.UsedSize / info.TotalSize;
            }
            return info;
        }

        /// <summary>
        /// Linux下取目录大小信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static DiskInfo LinuxFolderDiskInfo(string path)
        {
            DiskInfo disk = new DiskInfo();
            if (string.IsNullOrEmpty(path))
            {
                return disk;
            }
            if (!path.StartsWith("/"))
            {
                path = $"/{path}";
            }

            Process p = new Process();
            p.StartInfo.FileName = "sh";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine($"cd {path}");
            p.StandardInput.WriteLine($"df -k {path} | awk '{{print $2,$3,$4,$5}}'");
            p.StandardInput.WriteLine("exit");

            string strResult = p.StandardOutput.ReadToEnd();

            string[] arr = strResult.Split('\n');
            if (arr.Length == 0)
            {
                return disk;
            }
            string[] resultArray = arr[1].TrimStart().TrimEnd().Split(' ');
            if (resultArray == null || resultArray.Length == 0)
            {
                return disk;
            }

            disk.TotalSize = Convert.ToInt32(resultArray[0]) / (1024 * 1024);
            disk.UsedSize = Convert.ToInt32(resultArray[1]) / (1024 * 1024);
            disk.AvailableSize = Convert.ToInt32(resultArray[2]) / (1024 * 1024);
            disk.Use = disk.UsedSize / disk.TotalSize;

            return disk;
        }
        /// <summary>
        /// 磁盘空间
        /// </summary>
        public class DiskInfo
        {
            /// <summary>
            /// 总大小
            /// </summary>
            public long TotalSize { get; set; }
            /// <summary>
            /// 已使用
            /// </summary>
            public long UsedSize { get; set; }

            /// <summary>
            /// 剩余空间
            /// </summary>
            public long AvailableSize { get; set; }

            /// <summary>
            /// 使用率
            /// </summary>
            public decimal Use { get; set; }
        }
        #endregion
    }
}
