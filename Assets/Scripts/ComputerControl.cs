using UnityEngine;

public class ComputerControl : MonoBehaviour
{
    [SerializeField] private GameObject computerUI;
    [SerializeField] private GameObject desktop;
    [SerializeField] private GameObject mailbox;
    [SerializeField] private GameObject inbox;
    [SerializeField] private GameObject emailViewer;
    [SerializeField] private Movement playerMovement;
    [SerializeField] private NovaController novaController;
    [SerializeField] private DialogueData afterPCDialogue;

    // The email row that was opened
    private GameObject openedEmail;

    public void SetOpenedEmail(GameObject email)
    {
        openedEmail = email;
    }

    private void Update()
    {
        if (!computerUI.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ==========================================
            // EMAIL → INBOX
            // ==========================================
            if (emailViewer.activeSelf)
            {
                emailViewer.SetActive(false);
                inbox.SetActive(true);

                // Hide the email that was just opened
                if (openedEmail != null)
                {
                    openedEmail.SetActive(false);
                    openedEmail = null;
                }

                return;
            }

            // ==========================================
            // INBOX → DESKTOP
            // ==========================================
            if (mailbox.activeSelf)
            {
                mailbox.SetActive(false);
                desktop.SetActive(true);

                return;
            }

            // ==========================================
            // DESKTOP → CLOSE COMPUTER
            // ==========================================
            if (desktop.activeSelf)
            {
                // Check if all emails have been opened
                if (EmailManager.Instance != null &&
                    !EmailManager.Instance.AllEmailsOpened())
                {
                    Debug.Log(
                        "You must open all emails before closing the computer."
                    );

                    return;
                }

                // All emails opened → allow closing PC
                computerUI.SetActive(true);
                desktop.SetActive(false);

                playerMovement.enabled = true;

                novaController.ShowNova();

                if (DialogueManager.Instance != null &&
                    afterPCDialogue != null)
                {
                    DialogueManager.Instance.StartDialogue(
                        afterPCDialogue,
                        null
                    );
                }
            }
        }
    }
}