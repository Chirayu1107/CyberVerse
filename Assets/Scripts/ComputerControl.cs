using UnityEngine;

public class ComputerControl : MonoBehaviour
{
    [SerializeField] private Movement playerMovement;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
            playerMovement.enabled=true;
        }
    }
}