using UnityEngine;
using System.IO;
using SFB; // Make sure the StandaloneFileBrowser namespace is included

public class FileBrowserController : MonoBehaviour
{
    // Call this method when you want to open the file browser
    public void OpenFileBrowserAndCopyFile()
    {
        Debug.Log("Attempting to open file browser...");

        var extensions = new [] {
            new ExtensionFilter("Epub Files", "epub"),
            new ExtensionFilter("All Files", "*" ),
        };

        // Open the file browser and store the selected path
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);

        // Check if a file was selected
        if (paths.Length > 0)
        {
            string sourceFilePath = paths[0];
            Debug.Log($"File selected: {sourceFilePath}");
            CopyFileToStreamingAssets(sourceFilePath);
        }
        else
        {
            Debug.Log("No file selected.");
        }
    }

    private void CopyFileToStreamingAssets(string sourceFilePath)
    {
        // Extract the filename from the source file path
        string filename = Path.GetFileName(sourceFilePath);
        Debug.Log($"Filename extracted: {filename}");

        // Construct the destination path within the StreamingAssets folder
        string destinationPath = Path.Combine(Application.streamingAssetsPath, filename);
        Debug.Log($"Destination path: {destinationPath}");

        try
        {
            // Copy the file to the destination path
            File.Copy(sourceFilePath, destinationPath, overwrite: true);
            Debug.Log($"File copied to StreamingAssets: {destinationPath}");
        }
        catch (IOException ioEx)
        {
            // Handle exceptions (e.g., file not found, no permission, etc.)
            Debug.LogError($"IOException encountered: {ioEx.Message}");
        }
        catch (System.Exception ex)
        {
            // Handle any other exceptions
            Debug.LogError($"General Exception encountered: {ex.Message}");
        }
    }
}
