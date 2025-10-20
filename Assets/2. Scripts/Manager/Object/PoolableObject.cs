using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public ObjectManager.OBJECT type;

    public void ReturnToPool(){
        ObjectManager.Instance.ReturnObject(gameObject);
    }
}
