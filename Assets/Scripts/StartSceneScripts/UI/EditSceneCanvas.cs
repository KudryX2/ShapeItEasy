﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class EditSceneCanvas : ScriptableObject
{
    static Canvas canvas;
    static bool enabled;

    static Button saveButton, cancelEditingButton, deleteButton;    

    static InputField nameInputField, descriptionInputField;
    static bool clearInputFields;

    static Text notificationText;


    public static void Start()
    {
        canvas = GameObject.Find("EditSceneCanvas").GetComponent<Canvas>();

        /*
            Buttons
        */
        saveButton = GameObject.Find("SaveButton").GetComponent<Button>();
        saveButton.onClick.AddListener(() => saveButtonAction() );

        cancelEditingButton = GameObject.Find("EditSceneCancelButton").GetComponent<Button>();
        cancelEditingButton.onClick.AddListener(() => ScenesListCanvas.enable() );

        deleteButton = GameObject.Find("DeleteSceneButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(() => DeleteSceneConfirmationCanvas.enable() );

        /*  
            Input Fields 
        */
        nameInputField = GameObject.Find("EditSceneNameField").GetComponent<InputField>();
        descriptionInputField = GameObject.Find("EditSceneDescriptionField").GetComponent<InputField>();

        /*
            Notification
        */
        notificationText = GameObject.Find("EditSceneNotification").GetComponent<Text>();
        notificationText.text = "";
    }


    public static void Update()
    {
        canvas.enabled = enabled;

        if(clearInputFields){
            nameInputField.text = "";
            descriptionInputField.text = "";
            clearInputFields = false;
        }
    }

    public static void enable(Scene scene){      // Make visible
        nameInputField.text = scene.name;                       // Placeholder for name input field
        descriptionInputField.text = scene.description;         // Placeholder for description input field
        enabled = true;
        ScenesListCanvas.disable();
    }

    public static void disable(){                           // Hide
        enabled = false;
        clearInputFields = true;
    }


    private static void saveButtonAction(){

        if(nameInputField.text != "")
            Scenes.requestEditScene();
        else 
            notificationText.text = "Scene name field can´t be empty";

    }

    public static string getNameInput(){
        return nameInputField.text;
    }

    public static string getDescriptionInput(){
        return descriptionInputField.text;
    }


}
