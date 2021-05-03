using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems; // Event data.


public class ShareSceneButton : MonoBehaviour, IPointerDownHandler{

    public void OnPointerDown (PointerEventData eventData){
        Scene scene = Scenes.getScene(transform.parent.GetChild(0).GetComponent<Text>().text);  // Find scene by its name
        ShareSceneCanvas.enable(scene.shareViewID, scene.shareEditID);
    }
}
