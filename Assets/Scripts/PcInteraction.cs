using UnityEngine;

public class PcI : MonoBehaviour, IInteractable
{
  [SerializeField] private GameObject computerUI;
  [SerializeField] private Movement playerMovement;
    public void Interact()
    {
      computerUI.SetActive(true);
      playerMovement.enabled=false;
      Debug.Log("Computer Opened!");   
    }
}
