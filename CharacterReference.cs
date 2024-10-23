using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class CharacterReference : MonoBehaviour
{
    public CharacterInteraction interaction;

    [Header("UI References")]
    public  TextTyper textTyper;
    public TMP_InputField inputField; 
    public Button okButton; 

    // Call when player initiates an interaction
    public void StartInteraction()
    {
        if (interaction != null && textTyper != null && inputField != null && okButton != null)
        {
            // Just to pass elements to UI method of Interact
            interaction.Interact(textTyper, inputField, okButton);
        }
        else
        {
            Debug.LogError("Interaction or UI references not set on " + gameObject.name);
        }
    }

    public void EndInteraction()
    {
        if (interaction != null && textTyper.textField != null)
        {
            interaction.EndInteraction(textTyper.textField);
        }
    }
}
