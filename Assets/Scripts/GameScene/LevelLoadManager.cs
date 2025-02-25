using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoadManager : MonoBehaviour
{
    [SerializeField] private AllLevelsSO allLevels;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(allLevels.Levels.Find(x => x.ID == GameManager.Instance.CurrentLevel).LevelScenePrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
