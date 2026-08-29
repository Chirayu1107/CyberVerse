using UnityEngine;

public class Scene1ScenarioManager : MonoBehaviour
{
    public static Scene1ScenarioManager Instance { get; private set; }

    private bool scenarioCompleted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CompleteScenario()
    {
        scenarioCompleted = true;

        Debug.Log("SCENE 1 COMPLETED!");
    }

    public bool IsScenarioComplete()
    {
        return scenarioCompleted;
    }
}