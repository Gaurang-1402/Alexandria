using echo17.EndlessBook;
using UnityEngine;

public class InterMediaryController : MonoBehaviour
{
    public EndlessBook book; // Reference to the Endless Book plugin
    public PageRenderer2 pageRenderer; // Reference to the PageRenderer component

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

    // Example of updating the book when the user flips a page
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Get the next chunks of text from your text file based on charIndex
            string leftPageText = GetNextPageText(); // Implement this method to fetch the text for the left page
            string rightPageText = GetNextPageText(); // Implement this method to fetch the text for the right page
            AddNewPages(leftPageText, rightPageText);
        }
    }

    private string GetNextPageText()
    {
        // Your method to fetch the next chunk of text based on charIndex
        // Update this method to retrieve text appropriately
        return "Sample text for one page.";
    }
}
