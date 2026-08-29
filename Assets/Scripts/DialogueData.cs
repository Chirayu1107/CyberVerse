using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    [Header("Choice")]
    public string choiceId;

    [TextArea(1, 3)]
    public string choiceText;

    [Tooltip("Dialogue line to go to after selecting this choice. Use -1 to end dialogue.")]
    public int nextLineIndex = -1;

    [Header("Backend / Event Data")]
    public string eventId;

    [Header("Consequences")]
    public bool attackDetected = false;
    public bool attackSuccessful = false;
}


[System.Serializable]
public class DialogueBranch
{
    [Tooltip("GameState variable to check.")]
    public BranchVariable variable;

    [Tooltip("Expected value of the variable.")]
    public string value;

    [Tooltip("Dialogue line to jump to if the condition matches.")]
    public int nextLineIndex = -1;
}


public enum BranchVariable
{
    EmailDecision,
    PetDecision,
    CityDecision
}


[System.Serializable]
public class DialogueLine
{
    [Header("Dialogue")]
    public string speakerName;

    [TextArea(2, 5)]
    public string text;

    [Header("Player Choices")]
    public DialogueChoice[] choices;

    [Header("Automatic Branch")]
    public bool useBranch;

    public DialogueBranch[] branches;

    public bool endDialogueAfterLine;

    [Header("Automatic Jump")]
    public bool useAutomaticJump;

    [Tooltip("Line to jump to automatically after this line.")]
    public int automaticJumpLineIndex = -1;

    
}


[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public string npcName;

    public DialogueLine[] dialogueLines;
    [Header("Scenario")]
    [Tooltip("Enable this only for the final dialogue that completes the scene.")]
    public bool completesScene;
}