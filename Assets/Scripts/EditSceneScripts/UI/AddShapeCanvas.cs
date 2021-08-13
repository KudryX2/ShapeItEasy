using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class AddShapeCanvas : ScriptableObject
{
    private static Canvas canvas;
    private static bool enabled;

    private static Button sendButton, cancelButton;
    private static bool sendButtonEnabled;

    private static Dropdown shapeSelector;
    private static GameObject camera;

    // Start is called before the first frame update
    public static void Start()
    {
        canvas = GameObject.Find("AddShapeCanvas").GetComponent<Canvas>();

        sendButton = GameObject.Find("AddShapeSendButton").GetComponent<Button>();
        sendButton.onClick.AddListener(() => addShapeButton() );

        cancelButton = GameObject.Find("AddShapeCancelButton").GetComponent<Button>();
        cancelButton.onClick.AddListener(() => disable() );

        shapeSelector = GameObject.Find("ShapeSelector").GetComponent<Dropdown>();
        camera = GameObject.Find("Camera");    
    }

    // Update is called once per frame
    public static void Update()
    {
        canvas.enabled = enabled;
        sendButton.enabled = sendButtonEnabled;
    }

    public static void enable(){
        enabled = true;
        sendButtonEnabled = true;
    }

    public static void disable(){
        enabled = false;
        sendButtonEnabled = false;
    }

    private static void addShapeButton(){
        SceneEditor.enablePlacingShapeMode(shapeSelector.captionText.text);
        disable();
    }

}
