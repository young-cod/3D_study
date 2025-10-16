using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    public float speed = 7f;
    public float rotSpeed = 10f;

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            Vector3 rotDirection = player.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(rotDirection),
                Time.deltaTime * rotSpeed
                );
        }
    }
}
