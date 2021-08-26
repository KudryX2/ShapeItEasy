using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InfoCanvas : ScriptableObject
{

    static Text infoText;
    
    static bool needUpdate;

    static string positionString, tipsString;


    public static void Start(){
        infoText = GameObject.Find("InfoText").GetComponent<Text>();
    }

    public static void Update(){
        if(needUpdate){
            infoText.text = positionString + " | " + tipsString;
            needUpdate = false;
        }
    }

    public static void setPositionText(Vector3 position){
        positionString = "Position : " + position.x.ToString("0.##") + " " + position.y.ToString("0.##") + " " + position.z.ToString("0.##");
        needUpdate = true;
    }

    public static void setTipsText(string text){
        tipsString = text;
        needUpdate = true;
    }

}
