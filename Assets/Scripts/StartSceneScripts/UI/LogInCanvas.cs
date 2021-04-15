using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class LogInCanvas
{
    private static Canvas canvas;
    private static bool enabled;
    static Button logInButton, signInButton;

    public static void Start()
    {
        canvas =  GameObject.Find("LoginCanvas").GetComponent<Canvas>();

        logInButton = GameObject.Find("LogInButton").GetComponent<Button>();
        logInButton.onClick.AddListener(() => Session.logIn());

        signInButton = GameObject.Find("SignInButton").GetComponent<Button>();
        signInButton.onClick.AddListener(() => SignUpCanvas.enable());
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
