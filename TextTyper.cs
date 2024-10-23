using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI; 

public class TextTyper : MonoBehaviour
{
    public TMP_Text textField;  
    public ScrollRect scrollRect; 
    public float typingSpeed = 0.05f; 

    private Coroutine typingCoroutine;

    public void DisplayText(string textToType)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(textToType));
    }

    IEnumerator TypeText(string textToType)
    {
        //textField.text = "";  // Clear existing text
        foreach (char letter in textToType.ToCharArray())
        {
            textField.text += letter;  // Add one letter at a time
            yield return new WaitForSeconds(typingSpeed);  // Wait before adding the next letter
        }
        //StartCoroutine(ScrollToBottom());
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();  // Wait for end of frame so the UI elements can update
        scrollRect.verticalNormalizedPosition = 0f;  // Scroll to the bottom
        Debug.Log("Scrolled to the bottom."); // scrolling test 
    }
}
