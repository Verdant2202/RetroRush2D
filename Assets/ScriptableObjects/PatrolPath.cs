using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu()]
public class PatrolPath : ScriptableObject
{
    public List<Vector2> pathPoints;
    private void OnEnable()
    {
        if (pathPoints == null)
        {
            pathPoints = new List<Vector2>(); // Initialize the list if it is null
        }
    }
}
