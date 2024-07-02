using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnimations : MonoBehaviour
{
  
  public RectTransform panelTransform;
    public float slideDuration = 1f;
    public float slideDistance = 500f; // Distance to slide in from the right side

    private Vector3 originalPosition;
    private Vector3 slideInPosition;

    void Start()
    {
        if (panelTransform == null)
        {
            panelTransform = GetComponent<RectTransform>();
            if (panelTransform == null)
            {
                Debug.LogError("RectTransform component not found.");
                return;
            }
        }

        // Store the original position
        originalPosition = panelTransform.anchoredPosition;

        // Move the panel to the initial off-screen position (to the right)
        Vector3 startPos = originalPosition + new Vector3(slideDistance, 0f, 0f);
        slideInPosition = startPos;
        panelTransform.anchoredPosition = startPos;
    }

    public void SlideIn()
    {
        // Use LeanTween to slide in the panel from the right side
        LeanTween.move(panelTransform, originalPosition, slideDuration)
            .setEase(LeanTweenType.easeOutQuad); // Ease out for smooth transition
    }

    public void SlideOut()
    {
        // Use LeanTween to slide out the panel back to the right side
        LeanTween.move(panelTransform, slideInPosition, slideDuration)
            .setEase(LeanTweenType.easeOutQuad); // Ease out for smooth transition
    }

}
