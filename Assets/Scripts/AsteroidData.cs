using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/AsteroidData")]
public class AsteroidData : ScriptableObject
{
    public string name;
    public int pointValue;
    public Vector3 InitialForce;
    public bool spawnsMore;
    public bool spawnsPowerup;
}
