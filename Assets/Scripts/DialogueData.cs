using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;

    [Tooltip("Dialogue line to go to after selecting this choice. Use -1 to end dialogue.")]
    public int nextLineIndex = -1;

    [Header("Consequences")]
    public int securityScoreChange = 0;
    public bool countsAsMistake = false;
    public bool countsAsCorrect = false;
    public bool attackDetected = false;
    public bool attackSuccessful = false;
}

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string text;

    public DialogueChoice[] choices;

    public bool endDialogueAfterLine;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public string npcName;

    public DialogueLine[] dialogueLines;
}