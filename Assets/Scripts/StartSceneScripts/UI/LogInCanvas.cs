using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class LogInCanvas
{
    private static Canvas canvas;
    private static bool enabled;
    static Button logInButton, signUpButton, exitButton;

    public static void Start()
    {
        canvas =  GameObject.Find("LoginCanvas").GetComponent<Canvas>();

        logInButton = GameObject.Find("LogInButton").GetComponent<Button>();
        logInButton.onClick.AddListener(() => Session.logIn());

        signUpButton = GameObject.Find("SignUpButton").GetComponent<Button>();
        signUpButton.onClick.AddListener(() => SignUpCanvas.enable());

        exitButton = GameObject.Find("ExitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(() => Application.Quit() );
    }

    public static void Update(){
        canvas.enabled = enabled;
    }

    
    public static void enable(){        // Make Visible
        enabled = true;
        SignUpCanvas.disable();
        ScenesListCanvas.disable();
    }

    public static void disable(){       // Hide
        enabled = false;
    }
}
