using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{

    public const bool TESTING = false;

    public GameObject sceneListItem;

    void Start()
    {

        if(TESTING){
            RequestLoadTest.Start();
            PingTest.Start();

        }else{
            LogInCanvas.Start();
            SignUpCanvas.Start();

            Client.Start();
            Session.Start();

            ScenesListCanvas.Start(sceneListItem);
            CreateSceneCanvas.Start();
            EditSceneCanvas.Start();
            DeleteSceneConfirmationCanvas.Start(); 
            ShareSceneCanvas.Start();
            AddSceneCanvas.Start();
        }

    }


    void Update()
    {

        if(TESTING){


        }else{
            Scenes.Update();

            LogInCanvas.Update();
            SignUpCanvas.Update();

            Client.Update();

            ScenesListCanvas.Update();
            CreateSceneCanvas.Update();
            EditSceneCanvas.Update();
            DeleteSceneConfirmationCanvas.Update();
            ShareSceneCanvas.Update();
            AddSceneCanvas.Update();
        }

    }
}
