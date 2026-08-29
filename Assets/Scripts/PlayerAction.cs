using System;

[Serializable]
public class PlayerAction
{
    public string scenarioId;
    public string emailId;
    public string action;
    public bool isPhishing;
    public string timestamp;

    public PlayerAction(
        string scenarioId,
        string emailId,
        string action,
        bool isPhishing)
    {
        this.scenarioId = scenarioId;
        this.emailId = emailId;
        this.action = action;
        this.isPhishing = isPhishing;
        this.timestamp = DateTime.UtcNow.ToString("o");
    }
}