using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MissileController : MonoBehaviour
{
    List<GameObject> targets = new List<GameObject>();
    private Rigidbody rb;

    [Header("첫 미사일 관련")]
    public GameObject missilePrefab;
    public Transform muzzleTrs;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Fire();
        }
    }

    public void Fire()
    {
        GameObject missile = ObjectManager.Instance.GetObject(ObjectManager.OBJECT.Bullet); 
        //GameObject missile = Instantiate(missilePrefab, muzzleTrs.position,muzzleTrs.rotation);
    }

    

}
