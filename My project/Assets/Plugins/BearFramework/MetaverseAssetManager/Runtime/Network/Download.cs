
namespace Bear{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Net;
	using System.Net.Security;
	using System.Security.Cryptography.X509Certificates;
	using System.Threading;
	using UnityEngine;

	public class Download : CustomYieldInstruction
	{

		private readonly byte[] _readBuffer = new byte[DownloadSystem.ReadBufferSize];
		private long _bandWidth;

		public int _retryTimes;
		private Thread _thread;
		private FileStream _writer;

		private bool deleteFile;
		/// <summary>
		///     ftp 用户名
		/// </summary>
		public string FtpUserID { get; set; }

		/// <summary>
		///     ftp 用户密码
		/// </summary>
		public string FtpPassword { get; set; }
		public Download()
		{
			status = DownloadStatus.Wait;
			downloadedBytes = 0;
		}

		public bool retryEnabled { get; set; } = true;
		public DownloadInfo info { get; set; }
		public DownloadStatus status { get; set; }
		public string error { get; set; }
		public Action<Download> completed { get; set; }
		public Action<Download> updated { get; set; }

		public bool isDone => status == DownloadStatus.Failed || status == DownloadStatus.Success ||
		status == DownloadStatus.Canceled;

		public float progress => downloadedBytes * 1f / info.size;

		public long downloadedBytes { get; private set; }

		public override bool keepWaiting => !isDone;
		
		private void Retry()
		{
			status = DownloadStatus.Wait;
			Start();
		}
		
		public void UnPause()
		{
			Start();
		}

		public void Pause()
		{
			status = DownloadStatus.Wait;
		}

		public void Cancel()
		{
			error = "User Cancel.";
			status = DownloadStatus.Canceled;
		}

		public void Complete()
		{
			if (completed == null) return;
			completed.Invoke(this);
			completed = null;
		}

		
		private void Run()
		{
			lock (this)
			{
				try
				{
					deleteFile = true;

					Downloading();
					CloseWrite();

					if (status == DownloadStatus.Failed || status == DownloadStatus.Canceled || status == DownloadStatus.Wait)
						return;

					if (downloadedBytes != info.size)
					{
						if (deleteFile) File.Delete(info.savePath);
						error = $"Download lenght {downloadedBytes} mismatch to {info.size}";
						if (CanRetry()) return;

						status = DownloadStatus.Failed;
						return;
					}

					if (!string.IsNullOrEmpty(info.hash))
					{
						var hash = Utility.ComputeHash(info.savePath);
						if (info.hash != hash)
						{
							File.Delete(info.savePath);
							error = $"Download hash {hash} mismatch to {info.hash}";
							if (CanRetry()) return;

							status = DownloadStatus.Failed;
							return;
						}
					}

					status = DownloadStatus.Success;
				}
					catch (Exception e)
					{
						CloseWrite();
						error = e.Message;
						if (CanRetry()) return;

						status = DownloadStatus.Failed;
					}
			}
		}
		private void CloseWrite()
		{
			if (_writer == null) return;

			_writer.Flush();
			_writer.Close();
			_writer = null;
		}
		private bool CanRetry()
		{
			if (!retryEnabled) return false;

			if (_retryTimes >= DownloadSystem.MaxRetryTimes)
			{
				retryEnabled = false;
				return false;
			}

			Logger.W("Failed to download {0} with error {1}, auto retry {2}", info.url, error, _retryTimes);
			Thread.Sleep(1000);
			Retry();
			_retryTimes++;
			return true;
		}
		
		private void Downloading()
		{
			var file = new FileInfo(info.savePath);
			if (file.Exists && file.Length > 0)
			{
				if (info.size > 0 && file.Length == info.size)
				{
					status = DownloadStatus.Success;
					return;
				}

				_writer = File.OpenWrite(info.savePath);
				downloadedBytes = _writer.Length - 1;
				if (downloadedBytes > 0) _writer.Seek(-1, SeekOrigin.End);
			}
			else
			{
				var dir = Path.GetDirectoryName(info.savePath);
				if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
				_writer = File.Create(info.savePath);
				downloadedBytes = 0;
			}

			var request = CreateWebRequest();
			using (var response = request.GetResponse())
			{
				if (response.ContentLength > 0)
				{
					if (info.size == 0) info.size = response.ContentLength + downloadedBytes;

					using (var reader = response.GetResponseStream())
					{
						if (downloadedBytes >= info.size) return;

						var startTime = DateTime.Now;
						while (status == DownloadStatus.Progressing)
						{
							if (ReadToEnd(reader)) break;

							UpdateBandwidth(ref startTime);
						}
					}
				}
				else
				{
					status = DownloadStatus.Success;
				}
			}
		}
		
		private void UpdateBandwidth(ref DateTime startTime)
		{
			var average = DownloadSystem.MaxBandwidth / DownloadSystem.Progressing.Count;
			var elapsed = (DateTime.Now - startTime).TotalMilliseconds;
			while (DownloadSystem.MaxBandwidth > 0 &&
				status == DownloadStatus.Progressing &&
				_bandWidth >= average &&
				elapsed < 1000)
			{
				var wait = Mathf.Clamp((int) (1000 - elapsed), 1, 33);
				Thread.Sleep(wait);
				elapsed = (DateTime.Now - startTime).TotalMilliseconds;
			}

			if (!(elapsed >= 1000)) return;

			startTime = DateTime.Now;
			_bandWidth = 0L;
		}
		
		private WebRequest CreateWebRequest(){
			WebRequest request;
			if (info.url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
			{
				ServicePointManager.ServerCertificateValidationCallback = DownloadSystem.CheckValidationResult;
				request = GetHttpWebRequest();
			}else if(info.url.StartsWith("ftp", StringComparison.OrdinalIgnoreCase)){
				var ftpWebRequest = (FtpWebRequest) WebRequest.Create(info.url);
				ftpWebRequest.Method = WebRequestMethods.Ftp.DownloadFile;
				if (!string.IsNullOrEmpty(FtpUserID)) ftpWebRequest.Credentials = new NetworkCredential(FtpUserID, FtpPassword);

				if (downloadedBytes > 0) ftpWebRequest.ContentOffset = downloadedBytes;

				request = ftpWebRequest;

			}
			else
			{
				request = GetHttpWebRequest();
			}
			
			return request;

		}
		
		public WebRequest GetHttpWebRequest(){
			var request = (HttpWebRequest) WebRequest.Create(info.url);
			request.ProtocolVersion = HttpVersion.Version10;
			if (downloadedBytes > 0) request.AddRange(downloadedBytes);

			return request;

		}
		
		private bool ReadToEnd(Stream reader){
			var len = reader.Read(_readBuffer,0,_readBuffer.Length);
			if(len <= 0){
				if(downloadedBytes < info.size) deleteFile = false;
				return true;
			}
			
			_writer.Write(_readBuffer,0,len);
			downloadedBytes += len;
			_bandWidth += len;
			return false;

		}
		
		public void Start()
		{
			if (status != DownloadStatus.Wait) return;
			Logger.I("Start download {0}", info.url);
			status = DownloadStatus.Progressing;
			_thread = new Thread(Run)
			{
				IsBackground = true
            };
			_thread.Start();
		}
	}
}