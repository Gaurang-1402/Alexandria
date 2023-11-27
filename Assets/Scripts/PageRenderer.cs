using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PageRenderer : MonoBehaviour
{
    public Camera leftPageCamera; // Camera that renders the left page
    public Camera rightPageCamera; // Camera that renders the right page
    public TextMeshPro leftPageTextComponent; // TextMeshPro component on the left page
    public TextMeshPro rightPageTextComponent; // TextMeshPro component on the right page
    public RenderTexture leftPageRenderTexture; // RenderTexture for the left page
    public RenderTexture rightPageRenderTexture; // RenderTexture for the right page

    // Call this method to render the left page with given text and return a material
    public Material RenderLeftPageToMaterial(string text)
    {
        // Set the text for the left page
        leftPageTextComponent.text = text;

        // Render the left page to the RenderTexture
        leftPageCamera.targetTexture = leftPageRenderTexture;
        leftPageCamera.Render();

        // Create a new material for the left page
        Material leftPageMaterial = new Material(Shader.Find("Unlit/Texture"));
        leftPageMaterial.mainTexture = leftPageRenderTexture;

        return leftPageMaterial;
    }

    // Call this method to render the right page with given text and return a material
    public Material RenderRightPageToMaterial(string text)
    {
        // Set the text for the right page
        rightPageTextComponent.text = text;

        // Render the right page to the RenderTexture
        rightPageCamera.targetTexture = rightPageRenderTexture;
        rightPageCamera.Render();

        // Create a new material for the right page
        Material rightPageMaterial = new Material(Shader.Find("Unlit/Texture"));
        rightPageMaterial.mainTexture = rightPageRenderTexture;

        return rightPageMaterial;
    }
}

