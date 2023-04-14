using UnityEditor;

public static class ACToolExpansionAssetDatabase
{
    /// <summary>
    /// 重命名API
    /// </summary>
    /// <param name="pathName"></param>
    /// <param name="newName"></param>
    public static  void ACRenameAsset(this string pathName, string newName)
    {
        AssetDatabase.RenameAsset(pathName, newName);//改名API
    }
}
