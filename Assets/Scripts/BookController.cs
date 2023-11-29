 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VersOne.Epub;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using TMPro; // Include the TextMeshPro namespace if you're using TextMeshPro
using echo17.EndlessBook; // Include the EndlessBook namespace
using System.Text.RegularExpressions;

public class BookController : MonoBehaviour
{
    public EndlessBook book;
    public PageRenderer pageRenderer; // Reference to the PageRenderer component
    private int charIndex = 0; // Character index to keep track of text position
    private string fullBookContent; // Holds the entire text content of the book
    private List<int> pageStartIndices = new List<int>(); // Stores the start index of each page
    private int currentPageIndex = 0; // Tracks the current page index

    void Start()
    {
        // Read and parse the EPUB file
        EpubBook epubBook = EpubReader.ReadBook(Application.dataPath + "/Books/sapiens.epub");
        fullBookContent = ExtractContent(epubBook);

    }

    void Update()

        // TODO: put conditional for first page for flipping book cover
    {
        // Handle right arrow key press for turning pages forward
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Turn right");
            TurnPageRight();
        }

        // Handle left arrow key press for turning pages backward
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("Turn left");

            TurnPageLeft();
        }
    }

    private void TurnPageRight()


    {
        if (currentPageIndex < pageStartIndices.Count - 1)
        {
            currentPageIndex++;
            currentPageIndex++;
            Debug.Log("After page turn " + currentPageIndex);

            UpdatePageMaterials();
            book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 0.3f);

        }
        else if (IsAtLastPage())
        {
            Debug.Log("Last page");

            currentPageIndex++;
            currentPageIndex++;
            Debug.Log("After page turn " + currentPageIndex);

            AddNewPage();
            book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 0.3f);

        }
    }

    private void TurnPageLeft()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            currentPageIndex--;
            UpdatePageMaterials();

            Debug.Log("After page turn " + currentPageIndex);

            book.TurnToPage(book.CurrentLeftPageNumber - 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 0.3f);

        }
    }

    private void AddNewPage()
    {
        // HANDLES UPDATE PAGE MATERIAL ALSO

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
        string leftPageText = RenderPageBasedOnIndex(currentPageIndex - 2);
        string rightPageText = RenderPageBasedOnIndex(currentPageIndex - 1);

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
            return "INVALID PAGE NUMBER GIVEN"; // Return message for invalid page indices
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
