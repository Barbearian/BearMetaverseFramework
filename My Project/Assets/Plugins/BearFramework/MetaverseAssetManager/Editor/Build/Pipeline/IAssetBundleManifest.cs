namespace Bear.editor
{
    public interface IAssetBundleManifest
    {
        string[] GetAllAssetBundles();
        string[] GetAllDependencies(string assetBundle);
    }
}