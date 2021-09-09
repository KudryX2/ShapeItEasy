using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;


public class AddSceneCanvas : ScriptableObject
{

    private static Canvas canvas;
    private static bool enabled;

    private static Button addSceneButton, cancelButton;

    private static InputField idInputField;

    private static bool showErrorNotification;
    private static string errorNotification;


    public static void Start()
    {
        canvas = GameObject.Find("AddSceneCanvas").GetComponent<Canvas>();

        addSceneButton = GameObject.Find("AddSceneSendButton").GetComponent<Button>();
        addSceneButton.onClick.AddListener(() => Scenes.requestAddScene(idInputField.text) );

        cancelButton = GameObject.Find("AddSceneCancelButton").GetComponent<Button>();
        cancelButton.onClick.AddListener(() => cancel() );

        idInputField = GameObject.Find("AddSceneIdInput").GetComponent<InputField>();

    }

    public static void Update(){
        canvas.enabled = enabled;

        if(showErrorNotification){
            GameObject.Find("AddSceneNotification").GetComponent<Text>().text = errorNotification;  // Show notification 
            idInputField.text = "";                                                                 // Reset input field
            showErrorNotification = false;
        }
    }

    public static void setNotificationText(string notification){
        errorNotification = notification;
        showErrorNotification = true;
    }

    public static void enable(){        // Make visible
        enabled = true;
        ScenesListCanvas.disable();
    }

    public static void disable(){       // Hide
        enabled = false;
        showErrorNotification = true;   // Refresh input and notification
        errorNotification = "";
    }

    private static void cancel(){
        ScenesListCanvas.enable();
    }

    public static string getInputValue(){
        return idInputField.text;
    }
}

