using TMPro;
using UnityEngine;

public class InboxRowController : MonoBehaviour
{
    [SerializeField] private TMP_Text senderText;
    [SerializeField] private TMP_Text subjectText;
    [SerializeField] private TMP_Text timeText;

    private EmailData emailData;

    public void DisplayEmail(EmailData email)
    {
        if (email == null)
        {
            Debug.LogError("InboxRowController: Email data is null.");
            return;
        }

        emailData = email;

        senderText.text = email.senderName;
        subjectText.text = email.subject;

        timeText.text = email.date;
    }

    public EmailData GetEmailData()
    {
        return emailData;
    }
}