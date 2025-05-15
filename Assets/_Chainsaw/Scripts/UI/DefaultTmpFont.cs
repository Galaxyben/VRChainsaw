using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

/// <summary>
/// Sets the default TextMeshPro font to be used.
/// </summary>
[ExecuteInEditMode]
public class DefaultTmpFont : MonoBehaviour
{
    public LocalizedTmpFont localizedFont = new LocalizedTmpFont();
    public bool updateExistingText = true;

    void OnEnable() => localizedFont.AssetChanged += LocalizedFont_AssetChanged;
    void OnDisable() => localizedFont.AssetChanged -= LocalizedFont_AssetChanged;

    void LocalizedFont_AssetChanged(TMP_FontAsset value)
    {
        // Requires TMP 3.2 or greater
        TMP_Settings.defaultFontAsset = value;

        // Do we want to update any text that is already displayed with the new font?
        if (updateExistingText)
        {
            var o = FindObjectsOfType<TextMeshProUGUI>();
            foreach (var p in o)
            {
                p.font = null;
            }
        }
    }
}