using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class exampleCode : MonoBehaviour
{
    private void Awake()
    {
    Debug.Log($"Awake 호출");
    }
    private void OnEnable()
    {
        Debug.Log($"OnEnable 호출");


    }
    // Start is called before the first frame update
    void Start()
    {
    Debug.Log($"Start 호출");

    }
    private void FixedUpdate()
    {
    Debug.Log($"FixedUpdate 호출");

    }
    // Update is called once per frame
    void Update()
    {
    Debug.Log($"Update 호출");

    }
    private void LateUpdate()
    {
    Debug.Log($"LateUpdate 호출");

    }
    private void OnDisable()
    {
    Debug.Log($"OnDisable 호출");

    }

    private void OnDestroy()
    {
    Debug.Log($"OnDestroy 호출");

    }
}
