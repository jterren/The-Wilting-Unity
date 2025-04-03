#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public static class AddressableKeyAssignment
{

    [MenuItem("Tools/Addressables/Set Keys To Prefab Names")]
    public static void SetKeysToPrefabNames()
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

        if (settings == null)
        {
            Debug.LogError("Addressable settings not found.");
            return;
        }

        int renamedCount = 0;

        foreach (AddressableAssetGroup group in settings.groups)
        {
            if (group == null) continue;

            // Collect entries first to avoid modifying during iteration
            List<AddressableAssetEntry> entriesCopy = new List<AddressableAssetEntry>(group.entries);

            foreach (AddressableAssetEntry entry in entriesCopy)
            {
                if (entry == null || string.IsNullOrEmpty(entry.AssetPath))
                    continue;

                Object asset = AssetDatabase.LoadAssetAtPath<Object>(entry.AssetPath);
                if (asset == null)
                    continue;

                string assetName = asset.name;

                if (entry.address != assetName)
                {
                    Debug.Log($"Renaming key: {entry.address} → {assetName}");
                    entry.SetAddress(assetName);
                    renamedCount++;
                }
            }
        }

        settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryModified, null, true);
        AssetDatabase.SaveAssets();

        Debug.Log($"Addressables: Renamed {renamedCount} keys to prefab names.");
    }

}
#endif
