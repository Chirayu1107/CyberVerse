using UnityEngine;

public class NovaController : MonoBehaviour
{
    [Header("Nova")]
    [SerializeField] private GameObject novaVisual;

    [SerializeField] private Collider2D interactionZone;

    private void Awake()
    {
        HideNova();
    }

    public void ShowNova()
    {
        novaVisual.SetActive(true);

        if (interactionZone != null)
            interactionZone.enabled = true;
    }

    public void HideNova()
    {
        novaVisual.SetActive(false);

        if (interactionZone != null)
            interactionZone.enabled = false;
    }
}