using System;

[Serializable]
public class DialoguePlayerAction
{
    public string scenarioId;
    public string eventId;
    public string choiceId;
    public string timestamp;

    public DialoguePlayerAction(
        string scenarioId,
        string eventId,
        string choiceId)
    {
        this.scenarioId = scenarioId;
        this.eventId = eventId;
        this.choiceId = choiceId;
        this.timestamp =
            DateTime.UtcNow.ToString("o");
    }
}