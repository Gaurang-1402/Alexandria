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
    public Camera renderCamera; // Assign the camera that will render the page
    public TextMeshProUGUI textTemplate; // Assign a TextMeshProUGUI template
    public RenderTexture pageRenderTexture; // Assign a RenderTexture with the size matching the book page

    void Start()
    {
        // Read the ePub book
        EpubBook epubBook = EpubReader.ReadBook(Application.dataPath + "/Books/sapiens.epub");

        // Extract the text for the first page or any specific section
        string pageText = "An Animal of No Significance\n\nABOUT 13.5 BILLION YEARS AGO, MATTER, energy, ..."; // Shortened for brevity

        // Set the text to the TextMeshPro template
        textTemplate.text = pageText;

        // Render the text to the RenderTexture
        RenderTextToTexture(pageText);

        // Create a new material with the RenderTexture
        Material pageMaterial = new Material(Shader.Find("Unlit/Texture"));
        pageMaterial.mainTexture = pageRenderTexture;

        // Add the page material to the book
        book.AddPageData(pageMaterial);
    }

    void RenderTextToTexture(string text)
    {
        // Ensure the text template is active
        textTemplate.gameObject.SetActive(true);

        // Set the text you want to render
        textTemplate.text = text;

        // Set the camera's target texture to the render texture
        renderCamera.targetTexture = pageRenderTexture;

        // Render the camera's view (which includes the TextMeshPro text) to the texture
        renderCamera.Render();

        // Deactivate the text template if it should not stay visible
        textTemplate.gameObject.SetActive(false);
    }

    void Update()
    {

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
       } else if (Input.GetKeyDown(KeyCode.LeftArrow) && !book.IsFirstPageGroup) {
            // book.SetPageNumber(book.CurrentLeftPageNumber - 2);
            book.TurnToPage(book.CurrentLeftPageNumber - 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
       } else if (Input.GetKeyDown(KeyCode.RightArrow) && !book.IsLastPageGroup) {
            // book.SetPageNumber(book.CurrentLeftPageNumber + 2);
            book.TurnToPage(book.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
       }
        
    }
}
