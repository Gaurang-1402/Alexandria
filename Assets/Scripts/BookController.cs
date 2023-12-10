using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VersOne.Epub;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using TMPro;
using echo17.EndlessBook; // Include the EndlessBook namespace
using System.Text.RegularExpressions;
using UnityEngine.XR;
using UnityEngine.Networking;


public class BookController : MonoBehaviour
{
    public EndlessBook book;
    public PageRenderer pageRenderer; // Reference to the PageRenderer component
    private int charIndex = 0; // Character index to keep track of text position
    private string fullBookContent; // Holds the entire text content of the book
    private List<int> pageStartIndices = new List<int>(); // Stores the start index of each page
    private int currentLeftPageIndex = 0; // Tracks the current page index
                                          // Duration of the open/close animation
    public float openCloseTime = 1.0f; // 1 second for the animation


    // References to audio clips
    public AudioClip bookOpenClip;
    public AudioClip bookCloseClip;
    public AudioClip pageTurnClip;

    // AudioSource components that will play the audio clips
    private AudioSource bookOpenSound;
    private AudioSource bookCloseSound;
    private AudioSource pageTurnSound;
    void Start()
    {
        // Load audio clips
        LoadAudioClips();

        // Start coroutine to load EPUB file
        StartCoroutine(LoadEpubFromStreamingAssets());
    }

    void LoadAudioClips()
    {
        // Assuming the audio clips are located in a folder named "Sounds" within the Resources folder
        bookOpenClip = Resources.Load<AudioClip>("Sounds/BookOpen");
        bookCloseClip = Resources.Load<AudioClip>("Sounds/BookClose");
        pageTurnClip = Resources.Load<AudioClip>("Sounds/PageTurn");


        // Create and assign AudioSource components, set clip, and other properties
        bookOpenSound = gameObject.AddComponent<AudioSource>();
        bookOpenSound.clip = bookOpenClip;
        bookOpenSound.playOnAwake = false; // So it doesn't play immediately

        bookCloseSound = gameObject.AddComponent<AudioSource>();
        bookCloseSound.clip = bookCloseClip;
        bookCloseSound.playOnAwake = false; // So it doesn't play immediately

        pageTurnSound = gameObject.AddComponent<AudioSource>();
        pageTurnSound.clip = pageTurnClip;
        pageTurnSound.playOnAwake = false; // So it doesn't play immediately


    }

    IEnumerator LoadEpubFromStreamingAssets()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "sapiens.epub");
        UnityWebRequest request = UnityWebRequest.Get(filePath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Use the downloaded data as a byte array or convert it to a string
            byte[] epubBytes = request.downloadHandler.data;

            Debug.Log("EPUB loaded successfully");

            Debug.Log("EPUB size: " + epubBytes.Length);

            // Assuming you have a method that handles the byte array for the EPUB
            HandleEpubData(epubBytes);
        }
        else
        {
            Debug.LogError("Error loading EPUB: " + request.error);
        }
    }

    void HandleEpubData(byte[] epubData)
    {
        // Here you would convert the byte array to a stream and then to the EpubBook object
        // This is pseudocode and will need to be replaced with the actual implementation
        using (MemoryStream epubStream = new MemoryStream(epubData))
        {
            EpubBook epubBook = EpubReader.ReadBook(epubStream);
            fullBookContent = ExtractContent(epubBook);

            // Log the first 20 characters of the book content
            Debug.Log("Book content: " + (fullBookContent.Length >= 20 ? fullBookContent.Substring(0, 20) : fullBookContent));
        }
    }


private float inputDelay = 0.5f; // Time in seconds to wait between inputs
private float lastInputTime = 0f; // Time when the last input was registered

void Update()
{
    // Ensure enough time has passed since the last input
    if (Time.time - lastInputTime > inputDelay)
    {
        // Check for keyboard input
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandleRightTurn();
            lastInputTime = Time.time; // Update the last input time
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandleLeftTurn();
            lastInputTime = Time.time; // Update the last input time
        }

        // Check for VR controller input
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, inputDevices);

        foreach (var device in inputDevices)
        {
            if (device.TryGetFeatureValue(CommonUsages.primaryButton, out bool xButtonPressed) && xButtonPressed)
            {
                HandleRightTurn();
                lastInputTime = Time.time; // Update the last input time
            }
            else if (device.TryGetFeatureValue(CommonUsages.secondaryButton, out bool aButtonPressed) && aButtonPressed)
            {
                HandleLeftTurn();
                lastInputTime = Time.time; // Update the last input time
            }
        }
    }
}


    private void HandleRightTurn()
    {
        Debug.Log("Turn right");

        // Check if the book is closed
        if (book.CurrentState == EndlessBook.StateEnum.ClosedFront)
        {
            // Open the book and add the first set of pages
            OpenBookAndAddFirstPages();
        }
        else
        {
            // Otherwise, turn the page right
            TurnPageRight();
        }
    }
    private void HandleLeftTurn()
    {
        Debug.Log("Turn left");
        TurnPageLeft();
    }

    private void OpenBookAndAddFirstPages()
    {
        // Open the book
        book.SetState(EndlessBook.StateEnum.OpenMiddle, openCloseTime, OnBookStateChanged);
        pageTurnSound.Play();

        // currentLeftPageIndex += 2;

        if (pageStartIndices.Count == 0)
        {

            // Add the first set of pages
            // AddNewPage();
        }
        else
        {
            string leftPageText = RenderPageBasedOnIndex(currentLeftPageIndex);
            string rightPageText = RenderPageBasedOnIndex(currentLeftPageIndex + 1);

            // Render the text onto materials using the PageRenderer
            Material leftPageMaterial = pageRenderer.RenderLeftPageToMaterial(leftPageText);
            Material rightPageMaterial = pageRenderer.RenderRightPageToMaterial(rightPageText);

            // Update the materials for the current pages in the book
            // Assuming UpdatePageDataMaterial updates the material for a specific page in the EndlessBook
            book.UpdatePageDataMaterial(book.CurrentLeftPageNumber, leftPageMaterial);
            book.UpdatePageDataMaterial(book.CurrentRightPageNumber, rightPageMaterial);
            currentLeftPageIndex += 2;

        }


    }

    // Add the first set of pages
    // Get the text for the current left and right pages

    private void TurnPageRight()
    {
        if (currentLeftPageIndex < pageStartIndices.Count - 1)
        {
            currentLeftPageIndex += 2;
            Debug.Log("After page turn " + currentLeftPageIndex);

            UpdatePageMaterials();
            pageTurnSound.Play();

            book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 0.5f);
        }
        else if (IsAtLastPage())
        {

            const int charsPerLine = 38; // Set the fixed number of characters per line
            const int maxLinesPerPage = 16; // Set the maximum number of lines per page
            const int charsPerPage = charsPerLine * maxLinesPerPage; // Calculate total characters per page

            if (charIndex >= fullBookContent.Length || charIndex + charsPerPage >= fullBookContent.Length)
            {
                Debug.Log("Last page and end of content, close book");
                // Close the book because we reached the end of the content
                bookCloseSound.Play();
                book.SetState(EndlessBook.StateEnum.ClosedFront, openCloseTime, (fromState, toState, pageNumber) => OnBookStateChanged(fromState, toState, pageNumber));
                currentLeftPageIndex = 0;
                book.SetPageNumber(1);
            }
            else
            {
                Debug.Log("Last page but not end of content, add new page");

                currentLeftPageIndex += 2;
                Debug.Log("After page turn " + currentLeftPageIndex);

                AddNewPage();
                pageTurnSound.Play();
                book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 0.5f);
            }
        }
        else
        {
            Debug.Log("Closing book");

            bookCloseSound.Play();
            book.SetState(EndlessBook.StateEnum.ClosedFront, openCloseTime, (fromState, toState, pageNumber) => OnBookStateChanged(fromState, toState, pageNumber));
            currentLeftPageIndex = 0;
            book.SetPageNumber(1);
        }
    }



    // Method to handle book state changes
    private void OnBookStateChanged(EndlessBook.StateEnum fromState, EndlessBook.StateEnum toState, int pageNumber)
    {
        // Here you can handle what happens when the book state changes
        // For example, play a sound or update the UI
        Debug.Log("Book state changed from " + fromState + " to " + toState);
    }

    private void TurnPageLeft()
    {
        if (currentLeftPageIndex > 0)
        {
            currentLeftPageIndex -= 2;
            UpdatePageMaterials();
            Debug.Log("After page turn " + currentLeftPageIndex);
            book.TurnToPage(book.CurrentLeftPageNumber - 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 0.3f);
            pageTurnSound.Play();
        }
        else
        {
            // If we're at the first page, close the book
            if (book.CurrentState != EndlessBook.StateEnum.ClosedFront)
            {
                book.SetState(EndlessBook.StateEnum.ClosedFront, openCloseTime, OnBookStateChanged);
                bookCloseSound.Play();
            }
        }
    }



    private void AddNewPage()
    {

        // Get text for the next left and right pages
        string leftPageText = GetNextPageText();
        string rightPageText = GetNextPageText();

        // Check if there is text for the new pages
        if (!string.IsNullOrEmpty(leftPageText) || !string.IsNullOrEmpty(rightPageText))
        {
            // Render the text onto materials for left and right pages
            Material leftPageMaterial = pageRenderer.RenderLeftPageToMaterial(leftPageText);
            Material rightPageMaterial = pageRenderer.RenderRightPageToMaterial(rightPageText);

            // Add the rendered page materials to the book
            book.AddPageData(leftPageMaterial);
            book.AddPageData(rightPageMaterial);

        }
    }

    private void UpdatePageMaterials()
    {
        // Get the text for the current left and right pages
        string leftPageText = RenderPageBasedOnIndex(currentLeftPageIndex - 2);
        string rightPageText = RenderPageBasedOnIndex(currentLeftPageIndex - 1);

        // Render the text onto materials using the PageRenderer
        Material leftPageMaterial = pageRenderer.RenderLeftPageToMaterial(leftPageText);
        Material rightPageMaterial = pageRenderer.RenderRightPageToMaterial(rightPageText);

        // Update the materials for the current pages in the book
        // Assuming UpdatePageDataMaterial updates the material for a specific page in the EndlessBook
        book.UpdatePageDataMaterial(book.CurrentLeftPageNumber, leftPageMaterial);
        book.UpdatePageDataMaterial(book.CurrentRightPageNumber, rightPageMaterial);
    }

    private bool IsAtLastPage()
    {

        // Check if the current page is the last in the book
        return book.CurrentRightPageNumber >= book.LastPageNumber;
    }

    // Other methods (GetNextPageText, GetPrevPageText, RenderPageBasedOnIndex, ExtractContent, etc.) remain the same


    private string RenderPageBasedOnIndex(int pageIndex)
    {
        // Constants for text formatting
        const int charsPerLine = 38; // Fixed number of characters per line
        const int paragraphBreakFrequency = 6; // Lines after which to insert a paragraph break

        // Check if the page index is valid
        if (pageIndex < 0 || pageIndex >= pageStartIndices.Count)
        {
            book.SetState(EndlessBook.StateEnum.ClosedFront, openCloseTime, OnBookStateChanged);
            return "";
        }

        int startIndex = pageStartIndices[pageIndex];
        int endIndex = (pageIndex < pageStartIndices.Count - 1) ? pageStartIndices[pageIndex + 1] : fullBookContent.Length;

        // Extract the text for the current page
        string pageText = fullBookContent.Substring(startIndex, endIndex - startIndex);

        StringBuilder pageBuilder = new StringBuilder();
        int lineCounter = 0;

        // Loop through pageText in chunks of charsPerLine
        for (int i = 0; i < pageText.Length; i += charsPerLine)
        {
            int length = (i + charsPerLine > pageText.Length) ? pageText.Length - i : charsPerLine;
            pageBuilder.AppendLine(pageText.Substring(i, length));
            lineCounter++;

            // Add paragraph breaks based on the frequency
            if (lineCounter >= paragraphBreakFrequency && Random.Range(0, 2) > 0)
            {
                pageBuilder.AppendLine("\n");
                lineCounter = 0;
            }
        }

        return pageBuilder.ToString().TrimEnd(new char[] { ' ', '\n' }); // Trim trailing whitespace or newlines
    }



    private string GetNextPageText()
    {
        const int charsPerLine = 38; // Set the fixed number of characters per line
        const int maxLinesPerPage = 16; // Set the maximum number of lines per page
        const int charsPerPage = charsPerLine * maxLinesPerPage; // Calculate total characters per page
        const int paragraphBreakFrequency = 6; // Approximate number of lines after which to insert a paragraph break

        if (charIndex + charsPerPage > fullBookContent.Length)
            return null; // No more text to add

        // Store the start index of the current page
        pageStartIndices.Add(charIndex);

        string pageText = fullBookContent.Substring(charIndex, Mathf.Min(charsPerPage, fullBookContent.Length - charIndex));
        StringBuilder pageBuilder = new StringBuilder();
        int lineCounter = 0;

        // Loop through pageText in chunks of charsPerLine
        for (int i = 0; i < pageText.Length; i += charsPerLine)
        {
            int length = charsPerLine;
            if (i + length > pageText.Length) length = pageText.Length - i;

            pageBuilder.AppendLine(pageText.Substring(i, length));
            lineCounter++;

            if (lineCounter >= paragraphBreakFrequency && Random.Range(0, 2) > 0)
            {
                pageBuilder.AppendLine("\n");
                lineCounter = 0;
            }
        }

        // Update charIndex to the next set of characters
        charIndex += pageText.Length;

        return pageBuilder.ToString().TrimEnd(new char[] { ' ', '\n' }); // Trim any trailing whitespace or newlines
    }



    private string ExtractContent(EpubBook epubBook)
    {
        // Extract text from all reading order items and concatenate
        StringBuilder fullContent = new StringBuilder();
        foreach (EpubLocalTextContentFile textContentFile in epubBook.ReadingOrder)
        {
            string content = ExtractPlainText(textContentFile);
            // Remove all newlines and extra spaces
            string normalizedContent = Regex.Replace(content, @"\s+", " ");
            fullContent.Append(normalizedContent);
        }
        return fullContent.ToString();
    }

    private string ExtractPlainText(EpubLocalTextContentFile textContentFile)
    {
        HtmlDocument htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(textContentFile.Content);
        StringBuilder sb = new StringBuilder();
        foreach (HtmlNode node in htmlDocument.DocumentNode.SelectNodes("//text()"))
        {
            // Remove all newlines and carriage returns from the text node
            string text = node.InnerText.Trim();
            text = Regex.Replace(text, @"\r\n?|\n", " "); // Replace all types of newlines with a space
            sb.Append(text + " "); // Append text with a trailing space to separate paragraphs
        }
        // Replace occurrences of multiple spaces with two newlines to denote new paragraphs
        string content = Regex.Replace(sb.ToString(), @"[ ]{2,}", "\n\n");
        return content;
    }

}
