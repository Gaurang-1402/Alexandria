using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VersOne.Epub;
using System.Text;
using HtmlAgilityPack;
using System.IO;
using TMPro; // Include the TextMeshPro namespace if you're using TextMeshPro
using echo17.EndlessBook; // Include the EndlessBook namespace

public class BookController : MonoBehaviour
{
    public EndlessBook book;
    public PageRenderer pageRenderer; // Reference to the PageRenderer component
    private int charIndex = 0; // Character index to keep track of text position
    private string fullBookContent; // Holds the entire text content of the book

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
            book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
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
        if (charIndex + 100 > fullBookContent.Length)
            return null; // No more text to add

        string nextPageText = fullBookContent.Substring(charIndex, 100);
        charIndex += 100;
        return nextPageText;
    }

    private string ExtractContent(EpubBook epubBook)
    {
        // Extract text from all reading order items and concatenate
        StringBuilder fullContent = new StringBuilder();
        foreach (EpubLocalTextContentFile textContentFile in epubBook.ReadingOrder)
        {
            string content = ExtractPlainText(textContentFile);
            fullContent.Append(content);
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
            sb.AppendLine(node.InnerText.Trim());
        }
        return sb.ToString();
    }
}
