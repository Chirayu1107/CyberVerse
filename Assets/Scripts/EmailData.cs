using System;

[Serializable]
public class EmailData
{
    public string id;

    public string senderName;
    public string senderEmail;
    public string recipient;

    public string subject;
    public string date;
    public string body;
    public string linkText;

    public bool isPhishing;
    public bool isRead;
}