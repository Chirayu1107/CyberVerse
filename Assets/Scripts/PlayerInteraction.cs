using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject interactionPrompt;

    private IInteractable currentInteractable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null)
        {
            currentInteractable = interactable;
            interactionPrompt.SetActive(true);

            Debug.Log("Press E to interact");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();

        if (interactable != null && currentInteractable == interactable)
        {
            currentInteractable = null;
            interactionPrompt.SetActive(false);
        }
    }

    private void Update()
    {
      if (DialogueManager.Instance != null &&
        DialogueManager.Instance.IsDialogueActive)
      {
        return;
      }

      if (currentInteractable != null && Input.GetKeyDown(KeyCode.E))
      {
        currentInteractable.Interact();
      }
    }

    public void ClearInteraction()
    {
      currentInteractable = null;
      interactionPrompt.SetActive(false);
    }
}