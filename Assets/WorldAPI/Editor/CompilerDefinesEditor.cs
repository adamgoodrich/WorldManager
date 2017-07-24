using UnityEditor;

namespace WAPI
{
    /// <summary>
    /// Injects WORLDAPI_PRESENT define into project
    /// </summary>
    [InitializeOnLoad]
    class CompilerDefinesEditor : Editor
    {
        static CompilerDefinesEditor()
        {
            //Make sure we inject WORLDAPI_PRESENT
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (!symbols.Contains(WorldConstants.WAPIPresentSymbol))
            {
                symbols += ";" + WorldConstants.WAPIPresentSymbol;
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
            }
        }
    }
}