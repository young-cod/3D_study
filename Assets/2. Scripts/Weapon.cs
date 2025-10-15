using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int bulletCount;
    public Transform firePos;

    public GameObject Fire() => Instantiate(bulletPrefab, firePos);
}
