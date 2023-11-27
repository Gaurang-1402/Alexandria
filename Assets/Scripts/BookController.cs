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
        // Right arrow key to turn pages forward
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Check if we are at the last page and need to add new ones
            if (book.IsLastPageGroup)
            {
                // Get the next chunks of text from your text file
                string leftPageText = GetNextPageText(); // Get text for the left page
                string rightPageText = GetNextPageText(); // Get text for the right page

                // Only add new pages if there's text to add
                if (!string.IsNullOrEmpty(leftPageText) || !string.IsNullOrEmpty(rightPageText))
                {
                    AddNewPages(leftPageText, rightPageText);

                    // Since we're at the last page, we need to manually turn to the new pages
                    book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
                }
            }
            else
            {
                // If we're not at the last page group, just turn the page as usual
                book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
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
