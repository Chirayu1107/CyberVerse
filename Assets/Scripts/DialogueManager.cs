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
    private NPCInteraction currentNPC;

    private Coroutine typingCoroutine;
    private bool isTyping;


    private void Awake()
    {
        Instance = this;

        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);

        IsDialogueActive = false;
    }


    public void StartDialogue(
    DialogueData dialogue,
    NPCInteraction npc)
{
    if (dialogue == null)
    {
        Debug.LogWarning("DialogueData is null.");
        return;
    }

    currentDialogue = dialogue;
    currentNPC = npc;
    currentLine = 0;

    dialoguePanel.SetActive(true);
    IsDialogueActive = true;

    playerMovement.enabled = false;

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
        if (currentDialogue == null)
            return;

        if (currentLine < 0 ||
            currentLine >= currentDialogue.dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        DialogueLine line =
            currentDialogue.dialogueLines[currentLine];

        // -----------------------------
        // Automatic Branch
        // -----------------------------

        if (line.useBranch)
        {
            HandleAutomaticBranch(line);
            return;
        }

        // -----------------------------
        // Normal Dialogue
        // -----------------------------

        npcNameText.text = line.speakerName;

        choicePanel.SetActive(false);
        continueText.gameObject.SetActive(true);

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine =
            StartCoroutine(TypeLine(line.text));
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

        return choices != null &&
               choices.Length > 0;
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
                    choiceButtons[i]
                    .GetComponentInChildren<TMP_Text>();

                buttonText.text =
                    choices[i].choiceText;

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
            currentDialogue
            .dialogueLines[currentLine]
            .choices[choiceIndex];

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
        Debug.Log(
        $"[CHOICE SELECTED] EventID='{choice.eventId}' ChoiceID='{choice.choiceId}' Text='{choice.choiceText}'"
    );

    if (GameState.Instance == null)
    {
        Debug.LogWarning("GameState not found in scene.");
        return;
    }
        if (GameState.Instance == null)
        {
            Debug.LogWarning("GameState not found in scene.");
            return;
        }

        if (choice.attackDetected)
        {
            GameState.Instance.SetAttackDetected(true);
        }

        if (choice.attackSuccessful)
        {
            GameState.Instance.SetAttackSuccessful(true);
        }

        // Backend event
        // Track dialogue decision
    if (!string.IsNullOrEmpty(choice.eventId))
        {
        if (EventTracker.Instance != null)
            {
                EventTracker.Instance.TrackDialogueChoice(
                choice.eventId,
                choice.choiceId
                );
            }
            else
            {   
                Debug.LogWarning(
                "EventTracker not found."
                );
            }
        }
    }


    // =====================================================
    // GENERIC AUTOMATIC BRANCH SYSTEM
    // =====================================================

    private void HandleAutomaticBranch(DialogueLine line)
    {
        if (GameState.Instance == null)
        {
            Debug.LogWarning("GameState not found.");

            MoveToNextLine();
            return;
        }

        if (line.branches == null ||
            line.branches.Length == 0)
        {
            Debug.LogWarning(
                "Branch line has no branches configured."
            );

            MoveToNextLine();
            return;
        }

        foreach (DialogueBranch branch in line.branches)
        {
            string currentValue =
                GetGameStateValue(branch.variable);

            if (currentValue == branch.value)
            {
                Debug.Log(
                    $"Branch matched: {branch.variable} = {branch.value}"
                );

                currentLine = branch.nextLineIndex;

                ShowCurrentLine();

                return;
            }
        }

        Debug.LogWarning(
            "No matching dialogue branch found."
        );

        MoveToNextLine();
    }


    private string GetGameStateValue(BranchVariable variable)
    {
        switch (variable)
        {
            case BranchVariable.EmailDecision:

                return GameState.Instance
                    .emailDecision
                    .ToString();


            case BranchVariable.PetDecision:

                // We'll add this when Pet Decision is implemented.
                return "";


            case BranchVariable.CityDecision:

                // We'll add this when City Decision is implemented.
                return "";


            default:

                return "";
        }
    }


    private void MoveToNextLine()
    {
        currentLine++;

        if (currentLine >=
            currentDialogue.dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        ShowCurrentLine();
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

    // Automatic jump
    if (currentLineData.useAutomaticJump &&
        currentLineData.automaticJumpLineIndex >= 0)
    {
        currentLine = currentLineData.automaticJumpLineIndex;
        ShowCurrentLine();
        return;
    }

    MoveToNextLine();
}


    public void EndDialogue()
  {
    if (typingCoroutine != null)
    {
        StopCoroutine(typingCoroutine);
        typingCoroutine = null;
    }

    // Check whether THIS dialogue completes the scene
    if (currentDialogue != null &&
        currentDialogue.completesScene)
    {
        if (Scene1ScenarioManager.Instance != null)
        {
            Scene1ScenarioManager.Instance.CompleteScenario();
        }
        else
        {
            Debug.LogWarning(
                "Scene1ScenarioManager not found."
            );
         }
     }

      dialoguePanel.SetActive(false);
      choicePanel.SetActive(false);

      IsDialogueActive = false;
      isTyping = false;

      playerMovement.enabled = true;

      FindFirstObjectByType<PlayerInteraction>()
        ?.ClearInteraction();

      if (currentNPC != null)
      {
          currentNPC.OnDialogueFinished();
          currentNPC = null;
      }
   }
}