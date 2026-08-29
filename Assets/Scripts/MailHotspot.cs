using System;
using UnityEngine;

public class MailHotspot : MonoBehaviour
{
    [SerializeField] private GameObject desktop;
    [SerializeField] private GameObject mailbox;

    public void OpenMailbox()
    {
        desktop.SetActive(false);
        mailbox.SetActive(true);
    }

    public void CloseMailbox()
    {
        mailbox.SetActive(false);
        desktop.SetActive(true);
    }

    
}