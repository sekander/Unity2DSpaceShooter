using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Effects : MonoBehaviour
{

  public float slideSpeed = 500f; // Adjust slide speed as needed

    private RectTransform[] childRects; // Array to hold all child RectTransforms
    private bool[] hasSlidIn; // Array to track if each child panel has completed sliding in
    private int currentChildIndex = 0; // Index of the current child panel being animated

    void Start()
    {
        // Initialize childRects array and hasSlidIn flags
        InitializeChildRects(transform);
    }

    void InitializeChildRects(Transform parent)
    {
        int childCount = CountNestedChildren(parent);
        childRects = new RectTransform[childCount];
        hasSlidIn = new bool[childCount];

        int index = 0;
        foreach (Transform child in parent)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                Debug.Log("Child Init" + child.name);
                childRects[index] = rectTransform;
                // Initialize each panel offscreen to the left
                rectTransform.anchoredPosition = new Vector2(-rectTransform.rect.width, 0);
                hasSlidIn[index] = false;
                index++;
            }
            else {
                Debug.Log("Child is null " + child.name);
            }

            // Recursively initialize nested child panels
            InitializeChildRects(child);
        }
    }

    int CountNestedChildren(Transform parent)
    {
        int count = 0;
        foreach (Transform child in parent)
        {
            count++;
            // Recursively count nested child panels
            count += CountNestedChildren(child);
        }
        return count;
    }

    void OnEnable()
    {
        // Start the slide-in animation when the panel becomes visible
        SlideInRecursive(transform);
    }

    void SlideInRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Animate each panel to slide into view sequentially
                Debug.Log("Child Recursive  " + child.name);
                // StartCoroutine(SlideInPanel(rectTransform));
            }
            else {
            }
            // Recursively slide in nested child panels
            SlideInRecursive(child);
        }
    }

    IEnumerator SlideInPanel(RectTransform panel)
    {
        int index = System.Array.IndexOf(childRects, panel);
        if (index >= 0 && index < hasSlidIn.Length && !hasSlidIn[index])
        {
            while (panel.anchoredPosition.x < 0)
            {
                // Move panel towards the right until it's fully onscreen
                panel.anchoredPosition += new Vector2(slideSpeed * Time.deltaTime, 0);

                yield return null;
            }

            // Panel has fully slid in
            hasSlidIn[index] = true;
        }
    }
}
