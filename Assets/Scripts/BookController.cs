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


    void Start()
    {
        // Read and parse the EPUB file
        EpubBook epubBook = EpubReader.ReadBook(Application.dataPath + "/Books/sapiens.epub");
        fullBookContent = ExtractContent(epubBook);

        // Initialize the book with the first pages
        string leftPageText = GetNextPageText();
        string rightPageText = GetNextPageText();
        AddNewPages(leftPageText, rightPageText);
    }

    void Update()
    {
        // User input handling for page turning
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Fetch and add new pages if at the last page group
            if (book.IsLastPageGroup)
            {
                string leftPageText = GetNextPageText();
                string rightPageText = GetNextPageText();
                if (!string.IsNullOrEmpty(leftPageText) || !string.IsNullOrEmpty(rightPageText))
                {
                    AddNewPages(leftPageText, rightPageText);
                }
            }
            // Turn to the next page group
            book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 0.3f);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (book.CurrentLeftPageNumber > 0)
            {
                // Go back one page
                book.TurnToPage(book.CurrentLeftPageNumber - 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 0.3f);


            }
        }
    }

    private void AddNewPages(string leftPageText, string rightPageText)
    {
        Material leftPageMaterial = pageRenderer.RenderLeftPageToMaterial(leftPageText);
        Material rightPageMaterial = pageRenderer.RenderRightPageToMaterial(rightPageText);
        book.AddPageData(leftPageMaterial);
        book.AddPageData(rightPageMaterial);
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


    private string GetPrevPageText()
    {
        const int charsPerLine = 38; // Set the fixed number of characters per line
        const int maxLinesPerPage = 16; // Set the maximum number of lines per page
        const int charsPerPage = charsPerLine * maxLinesPerPage; // Calculate total characters per page

        if (pageStartIndices.Count <= 1) // Can't go back from the first page
            return null;

        // Calculate start index for the previous page
        int prevPageIndex = pageStartIndices[pageStartIndices.Count - 2];
        string prevPageText = fullBookContent.Substring(prevPageIndex, Mathf.Min(charsPerPage, charIndex - prevPageIndex));

        // Adjust the charIndex to the start of the previous page
        charIndex = prevPageIndex;

        // Remove the indices for the current page and the previous page
        pageStartIndices.RemoveAt(pageStartIndices.Count - 1);
        pageStartIndices.RemoveAt(pageStartIndices.Count - 1);

        return FormatPageText(prevPageText);
    }

    private string FormatPageText(string pageText)
    {
        const int charsPerLine = 38; // Fixed number of characters per line
        const int paragraphBreakFrequency = 6; // Lines after which to insert a paragraph break

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

        return pageBuilder.ToString().TrimEnd(new char[] { ' ', '\n' }); // Trim trailing whitespace or newlines
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
