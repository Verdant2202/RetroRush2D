using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LevelButtonsManager : MonoBehaviour
{
    [SerializeField] private AllLevelsSO allLevels;
    [SerializeField] private Transform gridLayoutGroup;
    [SerializeField] private GameObject levelButtonPrefab;
    // Start is called before the first frame update
    void Start()
    {
        foreach(LevelSO levelSO in allLevels.Levels)
        {
            GameObject button = Instantiate(levelButtonPrefab, gridLayoutGroup);
            button.GetComponent<LevelButton>().SetLevelSO(levelSO);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
