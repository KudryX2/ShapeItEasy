using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class LogInCanvas
{
    private static Canvas loginCanvas;
    private static bool enabled;
    static Button logInButton, signInButton;

    public static void Start()
    {
        loginCanvas =  GameObject.Find("LoginCanvas").GetComponent<Canvas>();

        logInButton = GameObject.Find("LogInButton").GetComponent<Button>();
        logInButton.onClick.AddListener(() => Session.logIn());

        signInButton = GameObject.Find("SignInButton").GetComponent<Button>();
        signInButton.onClick.AddListener(() => SignInCanvas.enable());
    }

    public static void Update(){
        loginCanvas.enabled = enabled;
    }

    public static void enable(bool newEnabled){
        enabled = newEnabled;
    }

    public static void enable(){
        enabled = true;
        SignInCanvas.enable(false);
    }
}
