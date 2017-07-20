using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

namespace WorldAPI
{
    /// <summary>
    /// Injects WORLDAPI_PRESENT define into project
    /// </summary>
    [InitializeOnLoad]
    public class CompilerDefinesEditor : Editor
    {
        static CompilerDefinesEditor()
        {
            //Make sure we inject CTS_PRESENT
            var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (!symbols.Contains(WorldConstants.WAPIPresentSymbol))
            {
                symbols += ";" + WorldConstants.WAPIPresentSymbol;
                PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, symbols);
            }
        }
    }
}