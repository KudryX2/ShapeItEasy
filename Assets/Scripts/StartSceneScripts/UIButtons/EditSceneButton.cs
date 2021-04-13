using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine.UI;  

using UnityEngine.EventSystems; // Event data.


public class EditSceneButton : MonoBehaviour, IPointerDownHandler{
    
    public void OnPointerDown (PointerEventData eventData){
        Scenes.makeEditSceneCanvasVisible(transform.parent.GetChild(0).GetComponent<Text>().text);  // Scene name
    }
}

