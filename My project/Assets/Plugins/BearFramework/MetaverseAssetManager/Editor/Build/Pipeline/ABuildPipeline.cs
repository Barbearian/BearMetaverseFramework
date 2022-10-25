using UnityEditor;

namespace Bear.editor
{
    public abstract class ABuildPipeline
    {
        public abstract IAssetBundleManifest BuildAssetBundles(string outputPath, AssetBundleBuild[] builds,
            BuildAssetBundleOptions options, BuildTarget target);
    }
}