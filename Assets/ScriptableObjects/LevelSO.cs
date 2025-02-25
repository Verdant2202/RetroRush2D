using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSO : ScriptableObject
{
    [SerializeField] private int _id;
    public int ID => _id;
    [SerializeField] private string _name;
    public string Name => _name;
    [SerializeField] private GameObject _levelScenePrefab;
    public GameObject LevelScenePrefab => _levelScenePrefab;
}
