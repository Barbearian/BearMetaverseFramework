using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear{
	using System;
	public class Downloader 
	{
		public Downloader(){
			DownloadURL = downloadURL;
			DownloadDataPath = $"{Application.persistentDataPath}/{Utility.buildPath}";
		}
		
		[Tooltip("资源下载地址，指向平台目录的父目录")] [SerializeField]
		private string downloadURL = "http://127.0.0.1/Bundles/";
		
		[Tooltip("最大并发下载数量")] [SerializeField] [Range(1, 10)]
		private uint maxDownloads;

		[Tooltip("单个下载最大带宽")] [SerializeField] private int maxDownloadSpeed = 1024 * 1024 * 4; // 4 MB

		[Tooltip("最大自修复异常次数")] [SerializeField] [Range(0, 5)]
		private int maxRetryTimes = 3;
		/// <summary>
		///     自定义下载地址
		/// </summary>
		public Func<string, string> CustomDownloader { get; set; }

		/// <summary>
		///     基本下载地址，需要指向平台资源目录的上层
		/// </summary>
		public string DownloadURL { get; set; }

		/// <summary>
		///     下载数据目录，所有使用 <see cref="GetDownloadURL" /> 下载的文件默认保存在此目录
		/// </summary>
		public string DownloadDataPath { get; private set; }
		
		
		/// <summary>
		///     获取指定文件相对下载目录的路径，默认会自动创建目录
		/// </summary>
		/// <param name="file">指定的文件的文件名</param>
		/// <returns>指定文件相对下载目录的路径</returns>
		public string GetDownloadDataPath(string file)
		{
			var path = $"{DownloadDataPath}/{file}";
			Utility.CreateDirectoryIfNecessary(path);
			return path;
		}
		/// <summary>
		///     获取指定文件的下载地址
		/// </summary>
		/// <param name="file">指定的文件的文件名</param>
		/// <returns>指定文件的下载地址</returns>
		public string GetDownloadURL(string file)
		{
			if (CustomDownloader == null) return $"{DownloadURL}{LoaderPathSystem.PlatformName}/{file}";

			var url = CustomDownloader(file);
			return !string.IsNullOrEmpty(url) ? url : $"{DownloadURL}{LoaderPathSystem.PlatformName}/{file}";
		}
	}
}