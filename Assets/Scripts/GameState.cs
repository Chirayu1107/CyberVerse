using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    [Header("Email Decision")]
    public EmailDecision emailDecision = EmailDecision.None;

    public enum EmailDecision
    {
        None,
        ReportedPhishing,
        RepliedWithCode
    }

    [Header("Scenario")]
    public bool attackDetected = false;
    public bool attackSuccessful = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetEmailDecision(EmailDecision decision)
    {
        emailDecision = decision;

        Debug.Log("Email Decision: " + emailDecision);
    }

    public void SetAttackDetected(bool value)
    {
        attackDetected = value;
    }

    public void SetAttackSuccessful(bool value)
    {
        attackSuccessful = value;
    }
}