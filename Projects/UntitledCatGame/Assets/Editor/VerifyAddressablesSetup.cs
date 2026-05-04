using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using System.IO;
using UnityEditor.AddressableAssets;

public class VerifyAddressablesSetup
{
    [MenuItem("Addressables/Verify Android Setup")]
    public static void VerifySetup()
    {
        Debug.Log("=== Verifying Addressables Android Setup ===");

        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("AddressableAssetSettings not found!");
            return;
        }

        // Check build settings
        Debug.Log($"Build Addressables with Player Build: {settings.BuildAddressablesWithPlayerBuild}");
        if (settings.BuildAddressablesWithPlayerBuild == AddressableAssetSettings.PlayerBuildOption.DoNotBuildWithPlayer)
        {
            Debug.LogWarning("Addressables are NOT set to build with player! You need to build them manually before building the player.");
        }
        else
        {
            Debug.Log("<color=green>Addressables will build automatically with player build.</color>");
        }

        // Check groups
        Debug.Log("\n=== Checking Groups ===");
        foreach (var group in settings.groups)
        {
            if (group == null) continue;

            Debug.Log($"\nGroup: {group.Name}");

            var schema = group.GetSchema<UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema>();
            if (schema != null)
            {
                Debug.Log($"  Include in Build: {schema.IncludeInBuild}");
                Debug.Log($"  Bundle Mode: {schema.BundleMode}");
                Debug.Log($"  Use Asset Bundle Cache: {schema.UseAssetBundleCache}");
                Debug.Log($"  Use Asset Bundle CRC: {schema.UseAssetBundleCrc}");

                var buildPath = settings.profileSettings.GetValueByName(settings.activeProfileId,
                    settings.profileSettings.GetProfileDataByName("Local.BuildPath").ToString());
                var loadPath = settings.profileSettings.GetValueByName(settings.activeProfileId,
                    settings.profileSettings.GetProfileDataByName("Local.LoadPath").ToString());

                Debug.Log($"  Build Path: {buildPath}");
                Debug.Log($"  Load Path: {loadPath}");

                if (!schema.IncludeInBuild)
                {
                    Debug.LogWarning($"  Group '{group.Name}' is NOT included in build!");
                }
            }
        }

        // Check if StreamingAssets exists
        string streamingAssetsPath = Application.dataPath + "/StreamingAssets";
        Debug.Log($"\n=== Checking Build Output ===");
        Debug.Log($"StreamingAssets Path: {streamingAssetsPath}");

        if (Directory.Exists(streamingAssetsPath))
        {
            string aaPath = streamingAssetsPath + "/aa";
            if (Directory.Exists(aaPath))
            {
                string androidPath = aaPath + "/Android";
                if (Directory.Exists(androidPath))
                {
                    var files = Directory.GetFiles(androidPath, "*.bundle", SearchOption.AllDirectories);
                    Debug.Log($"<color=green>Found {files.Length} bundle file(s) in StreamingAssets/aa/Android/</color>");
                    foreach (var file in files)
                    {
                        var fileInfo = new FileInfo(file);
                        Debug.Log($"  - {Path.GetFileName(file)} ({fileInfo.Length / 1024} KB)");
                    }

                    // Check for catalog
                    if (File.Exists(aaPath + "/Android/catalog.json"))
                    {
                        Debug.Log("<color=green>Catalog file found.</color>");
                    }
                    else
                    {
                        Debug.LogWarning("Catalog file NOT found!");
                    }
                }
                else
                {
                    Debug.LogWarning($"Android folder not found at: {androidPath}");
                    Debug.LogWarning("You need to build addressables for Android platform.");
                }
            }
            else
            {
                Debug.LogWarning($"Addressables folder not found at: {aaPath}");
                Debug.LogWarning("You need to build addressables first.");
            }
        }
        else
        {
            Debug.LogWarning("StreamingAssets folder does not exist!");
            Debug.LogWarning("Build addressables to create it automatically.");
        }

        Debug.Log("\n=== Verification Complete ===");
        Debug.Log("If bundles are missing from StreamingAssets:");
        Debug.Log("1. Go to Addressables > Build > New Build > Default Build Script");
        Debug.Log("2. Make sure your current platform is set to Android");
        Debug.Log("3. Verify bundles appear in Assets/StreamingAssets/aa/Android/");
    }
}
