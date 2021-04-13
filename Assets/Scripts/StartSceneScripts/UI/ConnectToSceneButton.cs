using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;  


using UnityEngine.EventSystems; // Event data.


public class ConnectToSceneButton : MonoBehaviour, IPointerDownHandler{

    public void OnPointerDown (PointerEventData eventData){
        Scenes.requestConnect(transform.parent.GetChild(0).GetComponent<Text>().text);  // Scene name
    }
}
