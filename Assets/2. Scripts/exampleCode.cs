using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class exampleCode : MonoBehaviour
{
    private void Awake()
    {
    Debug.Log($"Awake ȣ��");
    }
    private void OnEnable()
    {
        Debug.Log($"OnEnable ȣ��");


    }
    // Start is called before the first frame update
    void Start()
    {
    Debug.Log($"Start ȣ��");

    }
    private void FixedUpdate()
    {
    Debug.Log($"FixedUpdate ȣ��");

    }
    // Update is called once per frame
    void Update()
    {
    Debug.Log($"Update ȣ��");

    }
    private void LateUpdate()
    {
    Debug.Log($"LateUpdate ȣ��");

    }
    private void OnDisable()
    {
    Debug.Log($"OnDisable ȣ��");

    }

    private void OnDestroy()
    {
    Debug.Log($"OnDestroy ȣ��");

    }
}
