using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI; 

using UnityEngine.EventSystems; // Event data.


public class RemoveSceneButton : MonoBehaviour, IPointerDownHandler{

    public void OnPointerDown (PointerEventData eventData){
        string sceneName = transform.parent.GetChild(0).GetComponent<Text>().text;
        Scenes.setSelectedSceneID(sceneName);
        DeleteSceneConfirmationCanvas.enable();
    }
}
