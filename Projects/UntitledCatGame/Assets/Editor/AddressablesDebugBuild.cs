using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Build;
using System;
using UnityEditor.AddressableAssets;

public class AddressablesDebugBuild
{
    [MenuItem("Addressables/Build with Debug Logging")]
    public static void BuildWithDebugLogging()
    {
        try
        {
            Debug.Log("=== Starting Addressables Debug Build ===");

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogError("AddressableAssetSettings is NULL!");
                return;
            }
            Debug.Log($"Settings loaded: {settings.name}");

            Debug.Log($"Active Player Data Builder Index: {settings.ActivePlayerDataBuilderIndex}");
            Debug.Log($"Total Data Builders: {settings.DataBuilders.Count}");

            for (int i = 0; i < settings.DataBuilders.Count; i++)
            {
                var builder = settings.DataBuilders[i];
                if (builder == null)
                {
                    Debug.LogError($"Data Builder at index {i} is NULL!");
                }
                else
                {
                    Debug.Log($"Data Builder {i}: {builder.GetType().Name} - {builder.name}");
                }
            }

            var activeBuilder = settings.ActivePlayerDataBuilder;
            if (activeBuilder == null)
            {
                Debug.LogError("ActivePlayerDataBuilder is NULL! This is likely the cause of your error.");
                Debug.LogError($"Attempted to get builder at index {settings.ActivePlayerDataBuilderIndex}, but total builders = {settings.DataBuilders.Count}");
                return;
            }
            Debug.Log($"Active Builder: {activeBuilder.GetType().Name} - {activeBuilder.Name}");

            Debug.Log("=== Checking Asset Groups ===");
            foreach (var group in settings.groups)
            {
                if (group == null)
                {
                    Debug.LogError("Found NULL group in settings.groups!");
                    continue;
                }
                Debug.Log($"Group: {group.Name} (Entries: {group.entries.Count})");

                foreach (var entry in group.entries)
                {
                    if (entry == null)
                    {
                        Debug.LogError($"NULL entry in group {group.Name}");
                        continue;
                    }

                    var assetPath = AssetDatabase.GUIDToAssetPath(entry.guid);
                    if (string.IsNullOrEmpty(assetPath))
                    {
                        Debug.LogWarning($"Entry '{entry.address}' in group '{group.Name}' has invalid GUID: {entry.guid}");
                    }
                    else
                    {
                        var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                        if (asset == null)
                        {
                            Debug.LogWarning($"Entry '{entry.address}' in group '{group.Name}' points to missing asset at path: {assetPath}");
                        }
                    }
                }
            }

            Debug.Log("=== Starting Build ===");
            AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);

            if (result == null)
            {
                Debug.LogError("Build result is NULL!");
                return;
            }

            if (!string.IsNullOrEmpty(result.Error))
            {
                Debug.LogError($"Build Error: {result.Error}");
            }
            else
            {
                Debug.Log($"<color=green>Build Successful! Duration: {result.Duration}s</color>");
                Debug.Log($"Output Path: {result.OutputPath}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Exception during build: {ex.GetType().Name}");
            Debug.LogError($"Message: {ex.Message}");
            Debug.LogError($"Stack Trace:\n{ex.StackTrace}");

            if (ex.InnerException != null)
            {
                Debug.LogError($"Inner Exception: {ex.InnerException.GetType().Name}");
                Debug.LogError($"Inner Message: {ex.InnerException.Message}");
                Debug.LogError($"Inner Stack Trace:\n{ex.InnerException.StackTrace}");
            }
        }

        Debug.Log("=== Debug Build Complete ===");
    }
}
