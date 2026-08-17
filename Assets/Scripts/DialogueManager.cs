using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public bool IsDialogueActive { get; private set; }

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text continueText;

    [Header("Choice UI")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button[] choiceButtons;

    [Header("Typewriter")]
    [SerializeField] private float textSpeed = 0.03f;

    [Header("Player")]
    [SerializeField] private Movement playerMovement;

    private DialogueData currentDialogue;
    private int currentLine;

    private Coroutine typingCoroutine;
    private bool isTyping;

    private void Awake()
    {
        Instance = this;

        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);

        IsDialogueActive = false;
    }

    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        currentLine = 0;

        dialoguePanel.SetActive(true);
        IsDialogueActive = true;

        playerMovement.enabled = false;

        npcNameText.text = dialogue.npcName;

        ShowCurrentLine();
    }

    private void Update()
    {
        if (!IsDialogueActive)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                FinishTyping();
            }
            else if (!HasChoices())
            {
                NextLine();
            }
        }
    }

    private void ShowCurrentLine()
    {
        DialogueLine line = currentDialogue.dialogueLines[currentLine];

        choicePanel.SetActive(false);
        continueText.gameObject.SetActive(true);

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeLine(line.text));
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        typingCoroutine = null;

        ShowChoicesIfAvailable();
    }

    private void FinishTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueText.text =
            currentDialogue.dialogueLines[currentLine].text;

        isTyping = false;

        ShowChoicesIfAvailable();
    }

    private bool HasChoices()
    {
        DialogueChoice[] choices =
            currentDialogue.dialogueLines[currentLine].choices;

        return choices != null && choices.Length > 0;
    }

    private void ShowChoicesIfAvailable()
    {
        DialogueChoice[] choices =
            currentDialogue.dialogueLines[currentLine].choices;

        if (choices == null || choices.Length == 0)
        {
            choicePanel.SetActive(false);
            continueText.gameObject.SetActive(true);
            return;
        }

        continueText.gameObject.SetActive(false);
        choicePanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);

                TMP_Text buttonText =
                    choiceButtons[i].GetComponentInChildren<TMP_Text>();

                buttonText.text = choices[i].choiceText;

                int choiceIndex = i;

                choiceButtons[i].onClick.RemoveAllListeners();

                choiceButtons[i].onClick.AddListener(
                    () => SelectChoice(choiceIndex)
                );
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void SelectChoice(int choiceIndex)
{
    DialogueChoice choice =
        currentDialogue.dialogueLines[currentLine].choices[choiceIndex];

    ApplyChoiceConsequences(choice);

    choicePanel.SetActive(false);

    if (choice.nextLineIndex < 0)
    {
        EndDialogue();
        return;
    }

    currentLine = choice.nextLineIndex;

    ShowCurrentLine();
}

private void ApplyChoiceConsequences(DialogueChoice choice)
{
    if (GameState.Instance == null)
    {
        Debug.LogWarning("GameState not found in scene.");
        return;
    }

    if (choice.securityScoreChange != 0)
    {
        GameState.Instance.AddSecurityScore(
            choice.securityScoreChange
        );
    }

    if (choice.countsAsMistake)
    {
        GameState.Instance.RecordMistake();
    }

    if (choice.countsAsCorrect)
    {
        GameState.Instance.RecordCorrectDecision();
    }

    if (choice.attackDetected)
    {
        GameState.Instance.SetAttackDetected(true);
    }

    if (choice.attackSuccessful)
    {
        GameState.Instance.SetAttackSuccessful(true);
    }

    Debug.Log(
        $"Security Score: {GameState.Instance.securityScore}"
    );
}

    private void NextLine()
{
    DialogueLine currentLineData =
        currentDialogue.dialogueLines[currentLine];

    if (currentLineData.endDialogueAfterLine)
    {
        EndDialogue();
        return;
    }

    currentLine++;

    if (currentLine >= currentDialogue.dialogueLines.Length)
    {
        EndDialogue();
        return;
    }

    ShowCurrentLine();
}

    public void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);

        IsDialogueActive = false;
        isTyping = false;

        playerMovement.enabled = true;

        FindFirstObjectByType<PlayerInteraction>()?.ClearInteraction();
    }
}