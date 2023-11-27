using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VersOne.Epub;
using System.Text;
using HtmlAgilityPack;
using System.IO;

public class BookRenderer : MonoBehaviour
{
    void Start()
    {
        EpubBook epubBook = EpubReader.ReadBook(Application.dataPath + "/Books/sapiens.epub");

        // Log basic information
        Debug.Log("Title: " + epubBook.Title);
        Debug.Log("Author: " + epubBook.Author);

        // Log Navigation Items
        Debug.Log("Navigation Items:");
        foreach (EpubNavigationItem item in epubBook.Navigation)
        {
            Debug.Log("Navigation Title: " + item.Title);
        }

        // Extract and Log Reading Order Items
        Debug.Log("Reading Order Items:");
        StringBuilder fullContent = new StringBuilder();
        for (int i = 0; i < epubBook.ReadingOrder.Count; i++)
        {
            string content = ExtractPlainText(epubBook.ReadingOrder[i]);
            fullContent.AppendLine(content);
            Debug.Log("Reading Order " + (i + 1) + ": " + content.Substring(0, Mathf.Min(10000, content.Length)) + "...");
        }

        // Save to text file
        string outputPath = Application.dataPath + "/ExtractedContent.txt";
        File.WriteAllText(outputPath, fullContent.ToString());
        Debug.Log("Content saved to: " + outputPath);
    }

    string ExtractPlainText(EpubLocalTextContentFile textContentFile)
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
