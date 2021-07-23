using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AddShapePanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show(){
        gameObject.SetActive(true);
    }

    public void hide(){
        gameObject.SetActive(false);
    }

    public void addShapeButton(){
        Debug.Log("addShape");
        
        hide();
    }

    public void cancelButton(){
        hide();
    }
}
