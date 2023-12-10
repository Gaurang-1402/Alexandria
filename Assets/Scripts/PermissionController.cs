using UnityEngine;
using SimpleFileBrowser;

public class PermissionController : MonoBehaviour
{
    void Awake()
    {
#if !UNITY_EDITOR && UNITY_ANDROID

        // Request the MANAGE_EXTERNAL_STORAGE permission on Android 10+
        FileBrowser.Permission result = FileBrowser.CheckPermission();
        if (result == FileBrowser.Permission.ShouldAsk)
        {
            result = FileBrowser.RequestPermission();
            Debug.Log("Permission result: " + result);
        }

        if(result != FileBrowser.Permission.Granted)
        {
            Debug.LogError("Permission to access external storage was denied!");
            // Handle the lack of permission here
        }
#endif
    }
}
