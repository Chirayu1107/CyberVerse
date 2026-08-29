using UnityEngine;

public class EmailHotspot : MonoBehaviour
{
    private GameObject mailbox;
    private GameObject inbox;
    private GameObject emailViewer;

    private ComputerControl computerControl;
    private EmailViewerController emailController;

    private EmailData emailData;

    // Called by InboxController when the row is created
    public void Initialize(
        GameObject mailboxObject,
        GameObject inboxObject,
        GameObject emailViewerObject,
        ComputerControl computerControlObject,
        EmailViewerController emailControllerObject,
        EmailData email)
    {
        mailbox = mailboxObject;
        inbox = inboxObject;
        emailViewer = emailViewerObject;

        computerControl = computerControlObject;
        emailController = emailControllerObject;

        emailData = email;
    }

    public void OpenEmail()
    {
        if (emailData == null)
        {
            Debug.LogError("EmailHotspot: EmailData is missing.");
            return;
        }

        if (inbox == null)
        {
            Debug.LogError("EmailHotspot: Inbox reference is missing.");
            return;
        }

        if (emailViewer == null)
        {
            Debug.LogError("EmailHotspot: EmailViewer reference is missing.");
            return;
        }
        EmailManager.Instance.MarkEmailOpened(emailData);

        // Hide inbox
        inbox.SetActive(false);

        // Show full email
        emailViewer.SetActive(true);

        // Tell ComputerControl which ROW was opened
        InboxRowController rowController =
            GetComponentInParent<InboxRowController>();

        if (rowController != null)
        {
            computerControl.SetOpenedEmail(rowController.gameObject);
        }


        EventTracker.Instance.TrackEmailOpened(emailData);

        // Display email
        emailController.DisplayEmail(emailData);
    }
    
    public void CloseEmail()
    {
        // Close the email viewer
        emailViewer.SetActive(false);

        // Show the inbox again
        inbox.SetActive(true);

        // Hide only the email that was opened
        gameObject.SetActive(false);
    }
}