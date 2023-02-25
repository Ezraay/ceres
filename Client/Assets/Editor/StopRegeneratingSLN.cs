using System.IO;
using UnityEditor;
using UnityEngine;

public class StopRegeneratingSLN : EditorWindow
{
    private const string rootPath = "C:/Users/Valeriy/Documents/Projects/Project Card Game/Card Game/";
    private const string tempFileName = "Temp SLN.sln";
    private const bool enabled = false;
    private static string projectName => PlayerSettings.productName;

    private static void OnEnable()
    {
        if (enabled)
        {
            AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;
        }
    }

    private void OnDisable()
    {
        if (enabled) 
        {
            AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
        }
    }

    [InitializeOnLoadMethod]
    private static void OnLoad()
    {
        OnEnable();
    }

    private static void OnBeforeAssemblyReload()
    {
        Debug.Log("Before Assembly Reload");
        File.Copy(rootPath + "Card Game.sln", rootPath + tempFileName, true);
    }

    private static void OnAfterAssemblyReload()
    {
        Debug.Log("After Assembly Reload");
        File.Copy(rootPath + tempFileName, rootPath + "Card Game.sln", true);
        File.Delete(rootPath + tempFileName);
    }

    // static void Init()
    // {
    //     AssemblyReloadEvents.beforeAssemblyReload += BeforeAssemblyReload;
    //     AssemblyReloadEvents.afterAssemblyReload += AfterAssemblyReload;
    // }
    //
    // private void Awake()
    // {
    // }
    //
    // private static void AfterAssemblyReload()
    // {
    //     Debug.Log("After reload");
    //     File.Copy(Application.dataPath + "../Card Game.sln", Application.dataPath + "../Card Game2.sln");
    // }
    //
    // private static void BeforeAssemblyReload()
    // {
    //     Debug.Log("Before reload");
    // }
}