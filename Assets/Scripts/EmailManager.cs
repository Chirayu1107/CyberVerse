using System.Collections.Generic;
using UnityEngine;

public class EmailManager : MonoBehaviour
{
    [SerializeField] private InboxController inboxController;
    public static EmailManager Instance;

    private List<EmailData> emails = new List<EmailData>();
    private HashSet<string> openedEmailIds = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        CreateTestEmails();
    }

    private void CreateTestEmails()
    {
        emails.Clear();

        // Genuine email
        emails.Add(new EmailData
        {
            id = "email_001",
            senderName = "HR Department",
            senderEmail = "hr@company.com",
            recipient = "employee@company.com",
            subject = "Monthly Employee Benefits Update",
            date = "25 Aug 2026, 10:30 AM",
            body = "Hello,\n\nYour monthly employee benefits statement is now available.\n\nPlease review the information at your convenience.\n\nRegards,\nHR Department",
            linkText = "View Employee Benefits",
            isPhishing=false
        });

        // Phishing email
        emails.Add(new EmailData
        {
            id = "email_002",
            senderName = "IT Support",
            senderEmail = "it-support@company-security.com",
            recipient = "employee@company.com",
            subject = "Urgent: Verify Your Account",
            date = "25 Aug 2026, 09:15 AM",
            body = "Hello,\n\nYour account requires immediate verification.\n\nPlease verify your account using the link below to prevent your access from being suspended.\n\nRegards,\nIT Support",
            linkText = "Verify Your Account",
            isPhishing=true
        });
    }

    private void Start()
{
    inboxController.DisplayEmails(emails);
}

    public EmailData GetEmail(int index)
    {
        if (index < 0 || index >= emails.Count)
            return null;

        return emails[index];
    }

    public List<EmailData> GetAllEmails()
    {
        return emails;
    }

    public void MarkEmailOpened(EmailData email)
{
    if (email == null)
        return;

    openedEmailIds.Add(email.id);

    Debug.Log(
        "Email opened: " + email.id +
        " (" + openedEmailIds.Count +
        "/" + emails.Count + ")"
    );
}

public bool AllEmailsOpened()
{
    return emails.Count > 0 &&
           openedEmailIds.Count >= emails.Count;
}
}