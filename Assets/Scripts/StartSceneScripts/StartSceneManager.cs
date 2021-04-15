using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{

    public GameObject sceneListItem;

    void Start()
    {
        LogInCanvas.Start();
        SignUpCanvas.Start();

        Client.Start();
        Session.Start();

        ScenesListCanvas.Start(sceneListItem);
        CreateSceneCanvas.Start();
        EditSceneCanvas.Start();
    }


    void Update()
    {
        Scenes.Update();

        LogInCanvas.Update();
        SignUpCanvas.Update();

        ScenesListCanvas.Update();
        CreateSceneCanvas.Update();
        EditSceneCanvas.Update();
    }
}
