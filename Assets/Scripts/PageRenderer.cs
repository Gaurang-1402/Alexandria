using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PageRenderer : MonoBehaviour
{
    public Camera renderCamera; // Camera that renders the page
    public TextMeshPro textComponent; // TextMeshPro component on the page
    public RenderTexture renderTexture; // RenderTexture to capture the page
    public Material pageMaterialTemplate; // Template material for pages

    // Call this method to render a page with given text and return a material
    public Material RenderPageToMaterial(string text)
    {
        // Set the text
        textComponent.text = text;

        // Render the page to the RenderTexture
        renderCamera.targetTexture = renderTexture;
        renderCamera.Render();

        // Create a new material based on the template
        Material pageMaterial = new Material(pageMaterialTemplate);
        pageMaterial.mainTexture = renderTexture;

        return pageMaterial;
    }
}

