using UnityEngine;

public class NPCInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueData dialogueData;

    [Header("NPC Settings")]
    [SerializeField] private bool hideAfterDialogue = false;
    [SerializeField] private NovaController novaController;

    public void Interact()
    {
        if (dialogueData == null)
        {
            Debug.LogWarning(
                "No Dialogue Data assigned to " + gameObject.name
            );
            return;
        }

        DialogueManager.Instance.StartDialogue(
            dialogueData,
            this
        );
    }

    public void OnDialogueFinished()
    {
        if (!hideAfterDialogue)
            return;

        if (novaController != null)
        {
            novaController.HideNova();
        }
        else
        {
            Debug.LogWarning(
                "NovaController is not assigned to " + gameObject.name
            );
        }
    }
}