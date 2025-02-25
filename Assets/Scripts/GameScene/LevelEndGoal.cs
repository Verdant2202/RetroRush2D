using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndGoal : MonoBehaviour
{
    [SerializeField] private Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out Player hitPlayer))
        {
            if(hitPlayer == player)
            {
                player.WinLevel();
            }
        }
    }
}
