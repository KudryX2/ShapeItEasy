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
    }

    public static void enable(){
        enabled = true;
    }

    public static void disable(){
        enabled = false;
    }

    private static void addShapeButton(){

        Vector3 mouse = Input.mousePosition;            
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, camera.transform.position.y));
        Vector3 forward = mouseWorld + camera.transform.forward;        // In front of the camera
        
        SceneEditor.requestAddShape(shapeSelector.captionText.text, forward);
    }

}
