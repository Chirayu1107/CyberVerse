using UnityEngine;

public class EmailLinkHandler : MonoBehaviour
{
    private EmailData emailData;

    [SerializeField] private GameObject trapPanel;

    public void SetEmailData(EmailData email)
    {
        emailData = email;
    }

    public void ClickLink()
    {
        if (emailData == null)
        {
            Debug.LogError("EmailLinkHandler: No EmailData assigned.");
            return;
        }

        EventTracker.Instance.TrackLinkClicked(emailData);

        if (emailData.isPhishing)
        {
            Debug.LogWarning(
                "PLAYER GOT TRAPPED: " + emailData.id
            );

            // Store player's decision for dialogue branching
            if (GameState.Instance != null)
            {
                GameState.Instance.SetEmailDecision(
                    GameState.EmailDecision.RepliedWithCode
                );
            }

            trapPanel.SetActive(true);
        }
        else
        {
            Debug.Log(
                "Genuine email link clicked: " + emailData.id
            );
        }
    }

    public void CloseTrapPanel()
    {
        trapPanel.SetActive(false);
    }
}