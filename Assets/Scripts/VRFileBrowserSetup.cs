using UnityEngine;
using SimpleFileBrowser;

public class VRFileBrowserSetup : MonoBehaviour
{
    public Transform vrCameraTransform; // Assign your VR Camera Transform here in the inspector
    public float distanceInFrontOfCamera = 2.0f; // Distance to spawn the file browser in front of the camera

    void OnEnable()
    {
        // Subscribe to the OnShow event to be notified when the file browser is shown
        FileBrowser.OnShow += OnShowFileBrowser;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        FileBrowser.OnShow -= OnShowFileBrowser;
    }

    private void OnShowFileBrowser()
    {
        // Run this after a short delay to ensure the canvas is already instantiated
        Invoke(nameof(SetupFileBrowserCanvas), 0.1f);
    }

    private void SetupFileBrowserCanvas()
    {
        GameObject fileBrowserCanvas = GameObject.Find("SimpleFileBrowserCanvas(Clone)");
        if (fileBrowserCanvas != null)
        {
            // Set the canvas's position to be in front of the camera
            fileBrowserCanvas.transform.position = vrCameraTransform.position + vrCameraTransform.forward * distanceInFrontOfCamera;

            // Optionally, make the canvas face the camera directly
            fileBrowserCanvas.transform.forward = vrCameraTransform.forward;
        }
        else
        {
            Debug.LogWarning("SimpleFileBrowserCanvas(Clone) not found. Is the file browser open?");
        }
    }
}
