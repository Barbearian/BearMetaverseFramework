using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	using System.Security.Cryptography.X509Certificates;
	using System.Net.Security;

	public static class DownloadSystem
	{
		/// <summary>
		///     等待下载的队列
		/// </summary>
		private static readonly List<Download> Prepared = new List<Download>();

		/// <summary>
		///     下载中的队列，通过 <see cref="MaxDownloads" />> 控制并发数量
		/// </summary>
		private static readonly List<Download> Progressing = new List<Download>();

		/// <summary>
		///     下载失败的队列，在 <see cref="Progressing" /> 处理完后，会自动添加到 Prepared 中重新下载。
		/// </summary>
		private static readonly List<Download> Errors = new List<Download>();
		/// <summary>
		///     下载缓存，防止重复下载
		/// </summary>
		private static readonly Dictionary<string, Download> Cache = new Dictionary<string, Download>();
		private static float _lastSampleTime;
		private static long _lastTotalDownloadedBytes;
		
		/// <summary>
		///     最大并行下载数量
		/// </summary>
		public static uint MaxDownloads { get; set; } = 5;
		
		public static float AverageMaxBandwidth = DownloadSystem.MaxBandwidth / DownloadSystem.Progressing.Count;

		/// <summary>
		///     单个下载线程最大下载带宽，0 为限速，单位 byte
		/// </summary>
		public static long MaxBandwidth { get; set; }

		/// <summary>
		///     获取当前下载的总带宽，可以用来显示速度
		/// </summary>
		public static long TotalBandwidth { get; private set; }
		/// <summary>
		///     当前是否有下载任务
		/// </summary>
		public static bool Working => Progressing.Count > 0;
		
		/// <summary>
		///     当前总下载的字节数
		/// </summary>
		public static long TotalDownloadedBytes
		{
			get
			{
				var value = 0L;
				foreach (var item in Cache) value += item.Value.downloadedBytes;

				return value;
			}
		}
		/// <summary>
		///     当前总下载大小
		/// </summary>
		public static long TotalSize
		{
			get
			{
				var value = 0L;
				foreach (var item in Cache) value += item.Value.info.size;

				return value;
			}
		}
		
		/// <summary>
		///     自动重启下载的次数
		/// </summary>
		public static int MaxRetryTimes { get; set; } = 3;

		/// <summary>
		///     单线程单次IO读取缓冲大小
		/// </summary>
		public static uint ReadBufferSize { get; set; } = 1024 * 4;
		
		public static void ClearAllDownloads()
		{
			foreach (var download in Progressing) download.Cancel();

			Prepared.Clear();
			Progressing.Clear();
			Cache.Clear();
			Errors.Clear();
		}
		
		

		
		public static Download DownloadAsync(this Downloader downloader,string url, string savePath, Action<Download> completed = null,
			long size = 0, string hash = null)
		{
			return downloader.DownloadAsync(new DownloadInfo
			{
				url = url,
				savePath = savePath,
				hash = hash,
				size = size
            }, completed);
		}
		
		public static Download DownloadAsync(this Downloader downloader,DownloadInfo info, Action<Download> completed = null)
		{
			if (!Cache.TryGetValue(info.url, out var download))
			{
				download = new Download
				{
					info = info
                };
				Prepared.Add(download);
				Cache.Add(info.url, download);
			}
			else
			{
				Logger.W("Download url {0} already exist.", info.url);
			}

			if (completed != null) download.completed += completed;

			return download;
		}
		
		public static void UpdateAll()
		{
			if (Prepared.Count > 0)
				for (var index = 0; index < Mathf.Min(Prepared.Count, MaxDownloads - Progressing.Count); index++)
				{
					var download = Prepared[index];
					Prepared.RemoveAt(index);
					index--;
					Progressing.Add(download);
					download.Start();
				}

			if (Progressing.Count > 0)
			{
				for (var index = 0; index < Progressing.Count; index++)
				{
					var download = Progressing[index];
					download.updated?.Invoke(download);

					if (!download.isDone) continue;

					if (download.status == DownloadStatus.Failed)
					{
						if (download.retryEnabled) Errors.Add(download);

						Logger.E("Unable to download {0} with error {1}", download.info.url, download.error);
					}
					else
					{
						Logger.I("Success to download {0}", download.info.url);
					}

					Progressing.RemoveAt(index);
					index--;
					download.Complete();
				}

				if (!(Time.realtimeSinceStartup - _lastSampleTime >= 1)) return;

				TotalBandwidth = TotalDownloadedBytes - _lastTotalDownloadedBytes;
				_lastTotalDownloadedBytes = TotalDownloadedBytes;
				_lastSampleTime = Time.realtimeSinceStartup;
			}
			else
			{
				if (Cache.Count <= 0) return;

				Cache.Clear();
				if (Errors.Count > 0)
				{
					foreach (var download in Errors) Retry(download);

					Errors.Clear();
				}

				_lastTotalDownloadedBytes = 0;
				_lastSampleTime = Time.realtimeSinceStartup;
			}
		}
		
		public static void Retry(Download download)
		{
			download.status = DownloadStatus.Wait;
			download._retryTimes = 0;
			Prepared.Add(download);
			Cache[download.info.url] = download;
		}
		
		public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain,
			SslPolicyErrors spe)
		{
			return true;
		}
	}
}