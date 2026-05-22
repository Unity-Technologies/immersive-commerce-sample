using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

public class ValidateAddressablesPaths
{
    [MenuItem("Addressables/Validate All Paths")]
    public static void ValidatePaths()
    {
        Debug.Log("=== Validating Addressables Paths ===");

        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("AddressableAssetSettings not found!");
            return;
        }

        var profileSettings = settings.profileSettings;
        var activeProfileId = settings.activeProfileId;

        Debug.Log($"Active Profile ID: {activeProfileId}");

        // List all profile variables
        Debug.Log("\n=== Profile Variables ===");
        foreach (var varName in profileSettings.GetVariableNames())
        {
            var varId = profileSettings.GetProfileDataByName(varName);
            if (varId != null)
            {
                var value = profileSettings.GetValueByName(activeProfileId, varName);
                Debug.Log($"  {varName} (ID: {varId.Id}): {value}");
            }
        }

        // Check each group's paths
        Debug.Log("\n=== Validating Group Paths ===");
        foreach (var group in settings.groups)
        {
            if (group == null)
            {
                Debug.LogWarning("Found NULL group!");
                continue;
            }

            Debug.Log($"\n<color=cyan>Group: {group.Name}</color>");

            var schema = group.GetSchema<BundledAssetGroupSchema>();
            if (schema == null)
            {
                Debug.Log("  No BundledAssetGroupSchema found");
                continue;
            }

            Debug.Log($"  Include in Build: {schema.IncludeInBuild}");

            // Check Build Path
            var buildPathId = schema.BuildPath.Id;
            Debug.Log($"  BuildPath ID: {buildPathId}");

            try
            {
                var buildPathVarName = profileSettings.GetProfileDataById(buildPathId);
                if (buildPathVarName == null)
                {
                    Debug.LogError($"  <color=red>ERROR: BuildPath variable with ID '{buildPathId}' NOT FOUND in profile!</color>");
                }
                else
                {
                    Debug.Log($"  BuildPath Variable Name: {buildPathVarName.ProfileName}");
                    var buildPathValue = profileSettings.GetValueById(activeProfileId, buildPathId);
                    if (string.IsNullOrEmpty(buildPathValue))
                    {
                        Debug.LogError($"  <color=red>ERROR: BuildPath value is NULL or empty!</color>");
                    }
                    else
                    {
                        Debug.Log($"  BuildPath Value: {buildPathValue}");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"  <color=red>ERROR getting BuildPath: {e.Message}</color>");
            }

            // Check Load Path
            var loadPathId = schema.LoadPath.Id;
            Debug.Log($"  LoadPath ID: {loadPathId}");

            try
            {
                var loadPathVarName = profileSettings.GetProfileDataById(loadPathId);
                if (loadPathVarName == null)
                {
                    Debug.LogError($"  <color=red>ERROR: LoadPath variable with ID '{loadPathId}' NOT FOUND in profile!</color>");
                }
                else
                {
                    Debug.Log($"  LoadPath Variable Name: {loadPathVarName.ProfileName}");
                    var loadPathValue = profileSettings.GetValueById(activeProfileId, loadPathId);
                    if (string.IsNullOrEmpty(loadPathValue))
                    {
                        Debug.LogError($"  <color=red>ERROR: LoadPath value is NULL or empty!</color>");
                    }
                    else
                    {
                        Debug.Log($"  LoadPath Value: {loadPathValue}");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"  <color=red>ERROR getting LoadPath: {e.Message}</color>");
            }
        }

        Debug.Log("\n=== Validation Complete ===");
    }
}
