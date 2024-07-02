using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

// public class ButtonEvents : MonoBehaviour
public class ButtonEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Button button;
    private TextMeshProUGUI buttonText;
    private ColorBlock originalColors;
    private Color hoverColor = Color.yellow; // Color to change to on hover
    private Color originalColor;

const float max = 270f;
float start = 180f;
float end = 270f;
float current;

    void Start()
    {
        button = GetComponent<Button>();
          if (button == null)
        {
            Debug.LogError("Button component not found on the GameObject.");
            return;
        }

        // Get the TextMeshProUGUI component of the Button's text
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText == null)
        {
            Debug.LogError("TextMeshProUGUI component not found on the Button's child GameObject.");
            return;
        }

        // Store the original color of the button text
        // originalColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("Mouse entered button " + button.name);

        if(button.name == "Single_Player Button Text")
        {
            // Debug.Log("Mouse SINBGL entered button " + button.name);
            // Create a new ColorBlock to modify the highlighted color
            ColorBlock colorBlock = button.colors;
            colorBlock.normalColor = hoverColor;
            colorBlock.highlightedColor = hoverColor;
             // Store the original color of the button text
            originalColor = buttonText.color;

            // Apply the modified ColorBlock to the button
            // button.colors = colorBlock;
            // Change text color to hoverColor on hover
            buttonText.color = hoverColor;


            // // Fade to yellow color using LeanTween
            // LeanTween.color(button.image.rectTransform, Color.black, 0.5f)
            //     .setEase(LeanTweenType.easeOutQuad);
            //  LeanTween.value( button.gameObject, updateValueExampleCallback, 180f, 270f, 1f).setEase(LeanTweenType.easeOutElastic);
                // LeanTween.value( button.gameObject, updateValueExampleCallback, start, end, 1f).setEase(LeanTweenType.easeInOutSine);

          
        }
        else{
            // transform.LeanScale(Vector2.one, 0.8f);
            transform.LeanScale(new Vector2(1.5f, 1.5f), 0.8f);

        }


        // Add your hover effect logic here
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("Mouse exited button");
        if(button.name == "Single_Player Button Text")
        {
            // Debug.Log("Mouse SINBGL entered button " + button.name);
            buttonText.color = originalColor;
        }
        else{
            // transform.LeanScale(Vector2.zero , 1f).setEaseInBack();
            //  LeanTween.value( button.gameObject, updateValueExampleCallback, 270f, 180f, 1f).setEase(LeanTweenType.pingPong);
            transform.LeanScale(Vector2.one, 0.8f).setEaseInBack();
        }

        // Add your hover exit logic here
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Mouse button down on button");
        // Add your pointer down logic here
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Mouse button up on button");
        // Add your pointer up logic here
    }

    public void OnButtonClick()
    {
        Debug.Log("Button clicked");
        // Add your onClick logic here (if not already set in the Inspector)
    }

    void updateValueExampleCallback( float val ){
      Debug.Log("tweened value:"+val+" set this to whatever variable you are tweening...");
    }
}
