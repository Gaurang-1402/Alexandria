using System.Collections;
using UnityEngine;
using SimpleFileBrowser; // Ensure you have the correct namespace
using System.IO;

public class FileBrowserController : MonoBehaviour
{
    public GameObject buttonCanvas; // Assign your button canvas here in the inspector
    public GameObject fileBrowserCanvas; // Assign your button canvas here in the inspector

    public GameObject book; // Assign your epub reader here in the inspector
    // Call this function when the button is clicked
    public void OpenFileBrowser()
    {
        // Hide the button canvas
        if (buttonCanvas != null)
            buttonCanvas.SetActive(false);
        if (fileBrowserCanvas != null)
            fileBrowserCanvas.SetActive(true);

        StartCoroutine(ShowLoadDialogCoroutine());
    }

    private IEnumerator ShowLoadDialogCoroutine()
    {
        Debug.Log("Opening file browser");

        // Set default filter (optional)
        FileBrowser.SetDefaultFilter(".epub");

        // // Show the file browser and wait for a response from the user
        // // Only allow the selection of a single file
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Load Epub", "Load");

        // yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Load Epub", "Load");

        // Dialog is closed
        // Check if the user selected a file
        if (FileBrowser.Success)
        {
            // Get the path of the selected file
            string filePath = FileBrowser.Result[0];

            // Copy the selected file to the StreamingAssets folder
            CopyFileToStreamingAssets(filePath);
        }
        else Debug.Log("User canceled file browser");
    }

    private void CopyFileToStreamingAssets(string filePath)
    {
        string destinationPath = Path.Combine(Application.streamingAssetsPath, Path.GetFileName(filePath));

        Debug.Log("Destination path: " + destinationPath);
        try
        {
            if (!Directory.Exists(Application.streamingAssetsPath))
                Directory.CreateDirectory(Application.streamingAssetsPath);



            File.Copy(filePath, destinationPath, true);
            if (book != null)
            {
                book.SetActive(true);
            }
            Debug.Log("File copied to StreamingAssets: " + destinationPath);
        }
        catch (System.Exception ex)
        {
            if (book != null)
            {
                book.SetActive(true);
            }
            Debug.LogError("Error copying file: " + ex.Message);
        }
    }
}
