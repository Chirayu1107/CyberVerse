using TMPro;
using UnityEngine;

public class EmailViewerController : MonoBehaviour
{
    [Header("Email Text Fields")]
    [SerializeField] private TMP_Text subjectText;
    [SerializeField] private TMP_Text senderNameText;
    [SerializeField] private TMP_Text senderEmailText;
    [SerializeField] private TMP_Text recipientText;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text bodyText;
    [SerializeField] private TMP_Text linkText;
    [SerializeField] private EmailLinkHandler linkHandler;
    private EmailData currentEmail;

    public void DisplayEmail(EmailData email)
    {
        if (email == null)
        {
            Debug.LogError("EmailViewer: Email data is null.");
            return;
        }

        subjectText.text = email.subject;
        senderNameText.text = email.senderName;
        senderEmailText.text = email.senderEmail;
        recipientText.text = "To: " + email.recipient;
        dateText.text = email.date;
        bodyText.text = email.body;
        linkText.text = email.linkText;

        linkHandler.SetEmailData(email);
        currentEmail = email;
    }

    public void ReportEmail()
{
    if (currentEmail == null)
    {
        Debug.LogError("No email is currently open.");
        return;
    }

    // Store the player's decision for story branching
    if (GameState.Instance != null)
    {
        GameState.Instance.SetEmailDecision(
            GameState.EmailDecision.ReportedPhishing
        );
    }
    else
    {
        Debug.LogWarning("GameState not found.");
    }

    // Record the action for analytics/backend
    if (EventTracker.Instance != null)
    {
        EventTracker.Instance.TrackPhishingReported(currentEmail);
    }

    if (currentEmail.isPhishing)
    {
        Debug.Log("CORRECT ACTION: Phishing email reported.");
    }
    else
    {
        Debug.LogWarning("INCORRECT ACTION: Genuine email reported.");
    }
}
}