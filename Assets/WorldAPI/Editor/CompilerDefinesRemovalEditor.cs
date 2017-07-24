using UnityEditor;

namespace WAPI
{
    /// <summary>
    /// Remove World API compiler defines if deleted
    /// </summary>
    class WorldAPIRemovalEditor : UnityEditor.AssetModificationProcessor
    {
        public static AssetDeleteResult OnWillDeleteAsset(string AssetPath, RemoveAssetOptions rao)
        {
            if (AssetPath.Contains("WorldAPI"))
            {
                string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
                if (symbols.Contains(WorldConstants.WAPIPresentSymbol))
                {
                    symbols = symbols.Replace(WorldConstants.WAPIPresentSymbol + ";", "");
                    symbols = symbols.Replace(WorldConstants.WAPIPresentSymbol, "");
                    PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
                }
            }
            return AssetDeleteResult.DidNotDelete;
        }
    }
}