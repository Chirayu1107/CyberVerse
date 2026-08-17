using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueData dialogueData;

    public void Interact()
    {
        if (dialogueData == null)
        {
            Debug.LogWarning("No Dialogue Data assigned to " + gameObject.name);
            return;
        }

        DialogueManager.Instance.StartDialogue(dialogueData);
    }
}