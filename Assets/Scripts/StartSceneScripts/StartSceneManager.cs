using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{

    public GameObject sceneListItem;

    // Start is called before the first frame update
    void Start()
    {
        Client.Start();
        Session.Start();
        Scenes.Start(sceneListItem);
    }

    // Update is called once per frame
    void Update()
    {
        Session.Update();
        Scenes.Update();
    }
}
