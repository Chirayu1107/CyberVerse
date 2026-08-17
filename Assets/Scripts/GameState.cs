using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance;

    [Header("Security")]
    public int securityScore = 100;
    public int mistakes = 0;
    public int correctDecisions = 0;

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

    public void AddSecurityScore(int amount)
    {
        securityScore += amount;
        securityScore = Mathf.Clamp(securityScore, 0, 100);
    }

    public void RecordMistake()
    {
        mistakes++;
    }

    public void RecordCorrectDecision()
    {
        correctDecisions++;
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