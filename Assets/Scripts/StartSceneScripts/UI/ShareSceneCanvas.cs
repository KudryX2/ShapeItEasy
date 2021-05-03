using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class ShareSceneCanvas 
{
    private static Canvas canvas;
    private static bool enabled;
    private static InputField shareSceneViewID, shareSceneEditID;
    private static Button returnButton;


    public static void Start()
    {
        canvas = GameObject.Find("ShareSceneCanvas").GetComponent<Canvas>();

        returnButton = GameObject.Find("ShareSceneReturnButton").GetComponent<Button>();
        returnButton.onClick.AddListener(() => ScenesListCanvas.enable());   

        shareSceneViewID = GameObject.Find("ShareSceneViewID").GetComponent<InputField>();
        shareSceneEditID = GameObject.Find("ShareSceneEditID").GetComponent<InputField>();
    }

    public static void Update(){
        canvas.enabled = enabled;
    }


    public static void enable(string viewID, string editID){    // Make visible
        ScenesListCanvas.disable();

        shareSceneViewID.text = viewID;
        shareSceneEditID.text = editID;

        enabled = true;
    }

    public static void disable(){   // Hide
        enabled = false;
    }

}
