using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneCanvas.Start();   
        AddShapeCanvas.Start();
    }

    // Update is called once per frame
    void Update()
    {
        SceneCanvas.Update();
        AddShapeCanvas.Update();
    }
}
