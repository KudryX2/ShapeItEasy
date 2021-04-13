using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class SignInCanvas 
{
    private static Canvas signInCanvas;
    private static bool enabled;
    private static Button sendButton, cancelButton;

    public static void Start()
    {
        signInCanvas = GameObject.Find("SignInCanvas").GetComponent<Canvas>();

        sendButton = GameObject.Find("SendButton").GetComponent<Button>();
        sendButton.onClick.AddListener(() => Session.signIn());

        cancelButton = GameObject.Find("CancelButton").GetComponent<Button>();
        cancelButton.onClick.AddListener(() => LogInCanvas.enable());        
    }

    public static void Update(){
        signInCanvas.enabled = enabled;
    }


    public static void enable(bool newEnabled){
        enabled = newEnabled;
    }

    public static void enable(){
        enabled = true;
        LogInCanvas.enable(false);
    }

}
