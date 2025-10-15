using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;

    private void Update()
    {
        //rotation(x:90,0,0)
        transform.Translate(speed * Time.deltaTime * Vector3.up);
    }

}
