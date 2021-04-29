using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class SignUpCanvas 
{
    private static Canvas canvas;
    private static bool enabled;
    private static Button sendButton, cancelButton;

    static Text notificationTextComponent;
    static string notificationText = "";

    public static void Start()
    {
        canvas = GameObject.Find("SignUpCanvas").GetComponent<Canvas>();

        sendButton = GameObject.Find("SendButton").GetComponent<Button>();
        sendButton.onClick.AddListener(() => Session.signIn());

        cancelButton = GameObject.Find("CancelButton").GetComponent<Button>();
        cancelButton.onClick.AddListener(() => LogInCanvas.enable());   

        notificationTextComponent = GameObject.Find("SignUpNotifications").GetComponent<Text>();
        notificationTextComponent.text = "";     
    }

    public static void Update(){
        canvas.enabled = enabled;

        if(notificationText != ""){
            notificationTextComponent.text = notificationText;
            notificationText = "";
        }
    }


    public static void enable(){    // Make visible
        enabled = true;
        LogInCanvas.disable();
    }

    public static void disable(){   // Hide
        enabled = false;
    }


    public static void setNotificationText(string text){
        notificationText = text;
    }

}
