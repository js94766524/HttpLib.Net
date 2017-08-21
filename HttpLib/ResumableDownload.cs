using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace HttpLib
{
    /// <summary>
    /// 可断点续传的异步单线程文件下载工具类
    /// </summary>
    public class ResumableDownload : IDownloadHandler
    {
        private long _startPosition;
        private FileStream _fileStream;
        private Thread _workThread;

        public ResumableDownload(string url, string filePath)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                throw new ArgumentException(url);

            URL = url;
            FilePath = filePath;
        }
        
        /// <summary>
        /// 文件本地路径
        /// </summary>
        public string FilePath { get; }
        /// <summary>
        /// 文件网络地址
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 文件总长度
        /// </summary>
        public long FileLength { get; private set; }
        /// <summary>
        /// 已经下载的文件长度
        /// </summary>
        public long DoneLength { get; private set; }
        /// <summary>
        /// 下载进度百分比
        /// </summary>
        public double Progress { get; private set; }
        /// <summary>
        /// 指示下载是否在进行中
        /// </summary>
        public bool Downloading { get; private set; }

        /// <summary>
        /// 下载完毕事件
        /// </summary>
        public event EventHandler Finished;

        public event ProgressEventHandler ProgressChanged;

        /// <summary>
        /// 启动下载
        /// </summary>
        public void StartDownload()
        {
            if (Downloading) return;
            Downloading = true;

            InitialResumeParams();
            _workThread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    if(FileLength != 0 && _startPosition >= FileLength)
                    {
                        return;
                    }

                    URL.Download(this, _startPosition);
                    if(Progress == 100) Finished?.Invoke(this, new EventArgs());
                }
                catch (Exception e)
                {
                    throw new Exception("下载过程中出现异常", e);
                }
                finally
                {
                    Downloading = false;
                    _fileStream?.Close();
                }
            }));
            _workThread.Start();
        }

        /// <summary>
        /// 初始化下载所需要的参数和文件流对象
        /// </summary>
        private void InitialResumeParams()
        {
            if (File.Exists(FilePath))
            {
                _fileStream = File.OpenWrite(FilePath);
                _startPosition = _fileStream.Length;
                _fileStream.Seek(_startPosition, SeekOrigin.Current);
            }
            else
            {
                _startPosition = 0;
                _fileStream = new FileStream(FilePath, FileMode.Create);
            }
            DoneLength = _startPosition;
        }

        /// <summary>
        /// 处理下载的数据片段
        /// </summary>
        public void HandleBytes(byte[] data, int count)
        {
            _fileStream?.Write(data, 0, count);
            DoneLength += count;
            Progress = 100.0 * DoneLength / FileLength;
            ProgressChanged?.Invoke(this);
        }

        /// <summary>
        /// 计算文件总长度
        /// </summary>
        /// <param name="remineLength"></param>
        public void setFileLength(long remineLength)
        {
            FileLength = DoneLength + remineLength;
        }

        /// <summary>
        /// 停止下载
        /// </summary>
        public void Shutdown()
        {
            Downloading = false;
        }

        /// <summary>
        /// 重写ToString方法，配合Parse静态方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return URL + "," + FilePath;
        }

        /// <summary>
        /// 解析"URL,FilePath"格式的字符串为断线续传下载对象
        /// </summary>
        public static ResumableDownload Parse(string str)
        {
            string[] strs = str.Split(',');
            if (strs.Length != 3) return null;
            var r = new ResumableDownload(strs[0], strs[1]);
            r.FileLength = long.Parse(strs[2]);
            return r;
        }

        
    }

    public delegate void ProgressEventHandler(ResumableDownload sender);

    
}
