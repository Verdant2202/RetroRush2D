using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelButton : MonoBehaviour
{
    private LevelSO levelSO;
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private Button button;
    public void SetLevelSO(LevelSO lvlSO)
    {
        levelSO = lvlSO;
        levelNameText.text = levelSO.Name;
        button.onClick.AddListener(() => GameManager.Instance.LoadLevel(levelSO.ID));
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
