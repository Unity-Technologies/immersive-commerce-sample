using UnityEngine;
using UnityEditor;
using UnityEditor.AddressableAssets;

public class FixCommerceGroupPaths
{
    [MenuItem("Addressables/Fix Commerce Group Paths")]
    public static void FixPaths()
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("AddressableAssetSettings not found!");
            return;
        }

        Debug.Log("=== Fixing Commerce Group Paths ===");

        // The missing path IDs from the commerce packages
        string buildPathId = "3b97e878caae54470becd6724de1c2db";
        string loadPathId = "f5cdab40b5ada4697be193de7fcc318c";

        // Check if the paths already exist
        var existingBuildPath = settings.profileSettings.GetProfileDataById(buildPathId);
        var existingLoadPath = settings.profileSettings.GetProfileDataById(loadPathId);

        if (existingBuildPath == null)
        {
            Debug.Log("Adding missing BuildPath for commerce groups...");
            settings.profileSettings.CreateValue(buildPathId, "Commerce.BuildPath");

            // Set the value to use the same as Local.BuildPath for all profiles
            var profileIds = settings.profileSettings.GetAllProfileNames();
            foreach (var profileName in profileIds)
            {
                var profileId = settings.profileSettings.GetProfileId(profileName);
                settings.profileSettings.SetValue(profileId, "Commerce.BuildPath",
                    "[UnityEngine.AddressableAssets.Addressables.BuildPath]/[BuildTarget]");
            }
            Debug.Log("<color=green>Created Commerce.BuildPath</color>");
        }
        else
        {
            Debug.Log("Commerce.BuildPath already exists");
        }

        if (existingLoadPath == null)
        {
            Debug.Log("Adding missing LoadPath for commerce groups...");
            settings.profileSettings.CreateValue(loadPathId, "Commerce.LoadPath");

            // Set the value to use the same as Local.LoadPath for all profiles
            var profileIds = settings.profileSettings.GetAllProfileNames();
            foreach (var profileName in profileIds)
            {
                var profileId = settings.profileSettings.GetProfileId(profileName);
                settings.profileSettings.SetValue(profileId, "Commerce.LoadPath",
                    "{UnityEngine.AddressableAssets.Addressables.RuntimePath}/[BuildTarget]");
            }
            Debug.Log("<color=green>Created Commerce.LoadPath</color>");
        }
        else
        {
            Debug.Log("Commerce.LoadPath already exists");
        }

        // Save the settings
        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();

        Debug.Log("=== Fix Complete ===");
        Debug.Log("Try building addressables again.");
    }
}
