using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelButton : MonoBehaviour
{
    private LevelSO levelSO;
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    [SerializeField] private Color UnlockedColour;
    [SerializeField] private Color LockedColour;
    public void SetLevelSO(LevelSO lvlSO)
    {
        levelSO = lvlSO;
        levelNameText.text = levelSO.Name;
        if (GameManager.Instance.IsLevelUnlocked(levelSO.ID))
        {
            SetButtonVisual(true);
            button.onClick.AddListener(() => GameManager.Instance.LoadLevel(levelSO.ID));
        }
        else
        {
            SetButtonVisual(false);
        }
    }

    private void SetButtonVisual(bool enable)
    {
        if(enable)
        {
            image.color = UnlockedColour;
        }
        else
        {
            image.color = LockedColour;
        }
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
