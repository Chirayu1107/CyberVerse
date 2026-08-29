using UnityEngine;

public class PcI : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject computerUI;
    [SerializeField] private Movement playerMovement;
    [SerializeField] private PlayerInteraction playerInteraction;

    public void Interact()
    {
        computerUI.SetActive(true);
        playerMovement.enabled = false;

        if (playerInteraction != null)
        {
            playerInteraction.ClearInteraction();
        }

        Debug.Log("Computer Opened!");
    }

    public void CloseComputer()
    {
        computerUI.SetActive(false);
        playerMovement.enabled = true;
    }
}