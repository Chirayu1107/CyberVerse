using System.Collections.Generic;
using UnityEngine;

public class InboxController : MonoBehaviour
{
    [Header("Inbox Setup")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject emailRowPrefab;

    [Header("Scene References")]
    [SerializeField] private GameObject mailbox;
    [SerializeField] private GameObject inbox;
    [SerializeField] private GameObject emailViewer;
    [SerializeField] private ComputerControl computerControl;
    [SerializeField] private EmailViewerController emailController;

    private readonly List<GameObject> spawnedRows =
        new List<GameObject>();

    public void DisplayEmails(List<EmailData> emails)
    {
        ClearInbox();

        if (emails == null || emails.Count == 0)
        {
            Debug.LogWarning("InboxController: No emails to display.");
            return;
        }

        foreach (EmailData email in emails)
        {
            // Create email row
            GameObject row =
                Instantiate(emailRowPrefab, contentParent);

            // Get row controller
            InboxRowController rowController =
                row.GetComponent<InboxRowController>();

            if (rowController == null)
            {
                Debug.LogError(
                    "MailRowPrefab is missing InboxRowController."
                );

                Destroy(row);
                continue;
            }

            // Display inbox information
            rowController.DisplayEmail(email);

            // Find EmailHotspot
            EmailHotspot hotspot =
                row.GetComponentInChildren<EmailHotspot>();

            if (hotspot == null)
            {
                Debug.LogError(
                    "MailRowPrefab is missing EmailHotspot."
                );

                Destroy(row);
                continue;
            }

            // Give hotspot all scene references + email data
            hotspot.Initialize(
                mailbox,
                inbox,
                emailViewer,
                computerControl,
                emailController,
                email
            );

            spawnedRows.Add(row);
        }
    }

    private void ClearInbox()
    {
        foreach (GameObject row in spawnedRows)
        {
            if (row != null)
            {
                Destroy(row);
            }
        }

        spawnedRows.Clear();
    }
}