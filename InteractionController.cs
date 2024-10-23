using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    // references to UI elements
    public TMP_Text textField;
    public TMP_InputField inputField;
    public Button okButton;

    public TextTyper textTyper;

    public GameObject conversationCanvas; 
    public GameObject interactionPrompt; 

    public PlayerMovement playerMovement; // References to player control scripts 
    public FirstPersonLook playerMouseLook;

    public bool isInteracting = false;

    private GameObject currentInteractableObject = null;

    private void Update()
    {
        // Check if "E" is pressed and there is a character to interact with
        if (Input.GetKeyDown(KeyCode.E) && currentInteractableObject != null && !isInteracting)
        {
            StartInteraction();
        }

        // Check if the player is pressing the "ESC" key to end interaction
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInteracting)
            {
                EndInteraction();
            }
        }
    }

    private void StartInteraction()
    {
        CharacterReference interactable = currentInteractableObject.GetComponent<CharacterReference>();
        if (interactable != null && interactable.interaction != null)
        {
            interactable.interaction.Interact(textTyper, inputField, okButton); // Start interaction and config all settings
            interactionPrompt.SetActive(false);
            conversationCanvas.SetActive(true);
            playerMovement.canMove = false;
            playerMouseLook.ToggleLook(false);
            isInteracting = true;
        }
    }

    private void EndInteraction()
    {
        conversationCanvas.SetActive(false);
        ResetInteraction();
        playerMovement.canMove = true;
        playerMouseLook.ToggleLook(true);
        isInteracting = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Character")) 
        {
            currentInteractableObject = other.gameObject;
            interactionPrompt.SetActive(true); // Show the interaction prompt UI when the player is in range
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentInteractableObject)
        {
            currentInteractableObject = null;
            interactionPrompt.SetActive(false); // Hide the interaction prompt UI when the player is out of range
        }
    }

    private void ResetInteraction()
    {
        // Clear the text fields 
        textField.text = "";
        inputField.text = "";
        //inputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Enter your response...";
    }
}


