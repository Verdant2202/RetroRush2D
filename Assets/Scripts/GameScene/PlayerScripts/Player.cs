using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerVisual playerVisual;

    public async void WinLevel()
    {
        playerVisual.RunWinAnimation();
        await Task.Delay(1000);
        GameManager.Instance.BeatCurrentLevel();
        GameManager.Instance.QuitLevel();
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