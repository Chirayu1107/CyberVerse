using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private string sceneToLoad = "Scene2";

    public void Interact()
    {
        if (Scene1ScenarioManager.Instance == null)
        {
            Debug.LogError("Scene1ScenarioManager not found.");
            return;
        }

        if (!Scene1ScenarioManager.Instance.IsScenarioComplete())
        {
            Debug.Log("Scene 1 is not completed yet.");
            return;
        }

        Debug.Log("Scene 1 completed. Entering Scene 2...");

        SceneManager.LoadScene(sceneToLoad);
    }
}