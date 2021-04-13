using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{

    public GameObject sceneListItem;

    void Start()
    {
        LogInCanvas.Start();
        SignInCanvas.Start();

        Client.Start();
        Session.Start();
        Scenes.Start(sceneListItem);

    }


    void Update()
    {
        Scenes.Update();

        LogInCanvas.Update();
        SignInCanvas.Update();
    }
}
