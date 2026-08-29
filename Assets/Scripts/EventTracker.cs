using UnityEngine;

public class EventTracker : MonoBehaviour
{
    public static EventTracker Instance { get; private set; }

    [SerializeField] private string scenarioId = "scenario_01";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void TrackEmailOpened(EmailData email)
    {
        if (email == null)
            return;

        RecordAction(
            email,
            "email_opened"
        );
    }

    public void TrackLinkClicked(EmailData email)
    {
        if (email == null)
            return;

        RecordAction(
            email,
            "link_clicked"
        );
    }

    public void TrackPhishingReported(EmailData email)
    {
        if (email == null)
            return;

        RecordAction(
            email,
            "phishing_reported"
        );
    }

    private void RecordAction(
        EmailData email,
        string action)
    {
        PlayerAction playerAction =
            new PlayerAction(
                scenarioId,
                email.id,
                action,
                email.isPhishing
            );

        string json =
            JsonUtility.ToJson(playerAction);

        Debug.Log(
            "[PLAYER ACTION] " + json
        );
    }

    public void TrackDialogueChoice(
    string eventId,
    string choiceId)
{
    if (string.IsNullOrEmpty(eventId))
        return;

    DialoguePlayerAction action =
        new DialoguePlayerAction(
            scenarioId,
            eventId,
            choiceId
        );

    string json =
        JsonUtility.ToJson(action);

    Debug.Log(
        "[DIALOGUE ACTION] " + json
    );
}
}