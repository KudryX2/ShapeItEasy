using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class LogInCanvas
{
    static Canvas canvas;
    static bool enabled;
    static Button logInButton, signUpButton, exitButton;
    
    static Text notificationTextComponent;
    static string notificationText = "";

    public static void Start()
    {
        canvas =  GameObject.Find("LoginCanvas").GetComponent<Canvas>();

        logInButton = GameObject.Find("LogInButton").GetComponent<Button>();
        logInButton.onClick.AddListener(() => Session.logIn());

        signUpButton = GameObject.Find("SignUpButton").GetComponent<Button>();
        signUpButton.onClick.AddListener(() => SignUpCanvas.enable());

        exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(() => Application.Quit() );

        notificationTextComponent = GameObject.Find("LogInNotification").GetComponent<Text>();
        notificationTextComponent.text = "";
    }

    public static void Update(){
        canvas.enabled = enabled;

        if(notificationText != ""){
            notificationTextComponent.text = notificationText;
            notificationText = "";
        }
    }

    
    public static void enable(){        // Make Visible
        enabled = true;
        SignUpCanvas.disable();
        ScenesListCanvas.disable();
    }

    public static void disable(){       // Hide
        enabled = false;
        notificationText = " ";
    }

    public static void setNotificationText(string text){
        notificationText = text;        // We use the same variable to store the text to update and the need of action
    }
}
