using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using echo17.EndlessBook;
using VersOne.Epub;
using TMPro; // Include the TextMeshPro namespace if you're using TextMeshPro

public class BookController : MonoBehaviour
{
    public EndlessBook book;

    public PageRenderer pageRenderer; // Reference to the PageRenderer component
                                       // Call this to add new pages to the book
    public void AddNewPages(string leftPageText, string rightPageText)
    {
        // Render the pages and get the materials
        Material leftPageMaterial = pageRenderer.RenderLeftPageToMaterial(leftPageText);
        Material rightPageMaterial = pageRenderer.RenderRightPageToMaterial(rightPageText);

        // Add the materials to the book
        book.AddPageData(leftPageMaterial);
        book.AddPageData(rightPageMaterial);

        // Update the character index for next pages
    }

    void Update()
    {
        // Space key to toggle book open/close
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (book.CurrentState == EndlessBook.StateEnum.ClosedFront)
            {
                book.SetState(EndlessBook.StateEnum.OpenMiddle);
            }
            else
            {
                book.SetState(EndlessBook.StateEnum.ClosedFront);
            }
        }
        // Left arrow key to turn pages back
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && !book.IsFirstPageGroup)
        {
            book.TurnToPage(book.CurrentLeftPageNumber - 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
        }
        // Right arrow key to turn pages forward and add new pages if necessary
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!book.IsLastPageGroup)
            {
                book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
            }
            else
            {
                // Assume GetNextPageText will handle updating the charIndex and determining if there is more text to add
                string leftPageText = GetNextPageText(); // Implement this method to fetch the text for the left page
                string rightPageText = GetNextPageText(); // Implement this method to fetch the text for the right page

                // Check if there is new text to add before adding the pages
                if (!string.IsNullOrEmpty(leftPageText) || !string.IsNullOrEmpty(rightPageText))
                {
                    AddNewPages(leftPageText, rightPageText);
                }
            }
        }
    }

    private string GetNextPageText()
    {
        // Your method to fetch the next chunk of text based on charIndex
        // Update this method to retrieve text appropriately
        return "Sample text for one page.";
    }
}
