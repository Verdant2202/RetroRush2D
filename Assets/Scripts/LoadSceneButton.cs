using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneButton : MonoBehaviour
{
    public void LoadMainScene()
    {
        Loader.Load(Loader.Scene.MainScene);
    }

    public void LoadLevelSelectScene()
    {
        Loader.Load(Loader.Scene.LevelSelectScene);
    }

}
