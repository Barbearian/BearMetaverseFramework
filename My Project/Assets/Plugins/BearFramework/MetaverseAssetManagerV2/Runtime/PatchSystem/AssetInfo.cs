using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bear.Asset{
	public class AssetInfo 
	{
		private readonly ManifestAsset _patchAsset;
		private string _providerGUID;


		/// <summary>
		/// Is valid asset
		/// </summary>
		internal bool IsInvalid
		{
			get
			{
				return _patchAsset == null;
			}
		}
		
		/// <summary>
		/// Error Message
		/// </summary>
		internal string Error { private set; get; }

		/// <summary>
		/// address
		/// </summary>
		public string Address { private set; get; }

		/// <summary>
		/// asset path
		/// </summary>
		public string AssetPath { private set; get; }
		
		/// <summary>
		/// Asset type 
		/// </summary>
		public System.Type AssetType { private set; get; }
		
		/// <summary>
		/// Unique ID
		/// </summary>
		internal string GUID
		{
			get
			{
				if (!string.IsNullOrEmpty(_providerGUID))
					return _providerGUID;

				if (AssetType == null)
					_providerGUID = $"{AssetPath}[null]";
				else
					_providerGUID = $"{AssetPath}[{AssetType.Name}]";
				return _providerGUID;
			}
		}
		
		
		private AssetInfo()
		{
		}
		
		internal AssetInfo(ManifestAsset manifestAsset, System.Type assetType)
		{
			if (manifestAsset == null)
				throw new System.Exception("Should never get here !");

			_patchAsset = manifestAsset;
			AssetType = assetType;
			//Address = manifestAsset.Address;
			AssetPath = manifestAsset.path;
			Error = string.Empty;
		}
		
		internal AssetInfo(ManifestAsset patchAsset)
		{
			if (patchAsset == null)
				throw new System.Exception("Should never get here !");

			_patchAsset = patchAsset;
			AssetType = null;
			//Address = patchAsset.Address;
			AssetPath = patchAsset.path;
			Error = string.Empty;
		}
		
		internal AssetInfo(string error)
		{
			_patchAsset = null;
			AssetType = null;
			Address = string.Empty;
			AssetPath = string.Empty;
			Error = error;
		}
	}
}