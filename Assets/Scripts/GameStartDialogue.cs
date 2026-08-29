using UnityEngine;

public class GameStartDialogue : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;

    private void Start()
    {
        if (dialogueData == null)
        {
            Debug.LogWarning(
                "No Dialogue Data assigned to GameStartDialogue."
            );
            return;
        }

        if (DialogueManager.Instance == null)
        {
            Debug.LogWarning(
                "DialogueManager not found."
            );
            return;
        }

        DialogueManager.Instance.StartDialogue(
            dialogueData,
            null
        );
    }
}