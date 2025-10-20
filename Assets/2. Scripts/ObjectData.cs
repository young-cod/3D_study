using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectData
{
    public ObjectManager.OBJECT type;
    public Transform transform;
    public GameObject prefab;
    public int initialCount = 10;

    [HideInInspector] public Queue<GameObject> pool = new Queue<GameObject>();
} 
