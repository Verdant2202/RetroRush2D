using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int CurrentLevel;
    public static GameManager Instance;
    public List<int> unlockedLevels;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void LoadLevel(int level)
    {
        CurrentLevel = level;
        Loader.Load(Loader.Scene.PlayScene);
    }

    public void QuitLevel()
    {
        Loader.Load(Loader.Scene.LevelSelectScene);
    }

    public bool IsLevelUnlocked(int level)
    {
        if(unlockedLevels.Contains(level))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UnlockNextLevel()
    {
        UnlockLevel(CurrentLevel + 1);
    }

    public void UnlockLevel(int level)
    {
        if(IsLevelUnlocked(level))
        {
            return;
        }
        unlockedLevels.Add(level);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
