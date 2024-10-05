using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeWriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Assign your TextMeshProUGUI component in the Inspector
    public float typingSpeed = 0.02f;     // Delay between each character

    private string fullText;              // Stores the full text to be displayed

    void Start()
    {
        // Store the original text and clear it
        fullText = textComponent.text;
        textComponent.text = "";

        // Start typing the text
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        // Iterate over each character in the fullText and gradually reveal them
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    // Optional: A method to start typing from an external script
    public void StartTyping(string newText)
    {
        StopAllCoroutines();
        fullText = newText;
        textComponent.text = "";
        StartCoroutine(TypeText());
    }
}
