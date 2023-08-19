using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : SceneSingleton<MainGameManager>
{
    public GameController GameController;
    void Start()
    {
        ViewManager.Show(ViewId.GameView);
    }
}