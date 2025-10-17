using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//todo : 매번 생성하고 파괴할 수 없으니 풀을 만들도록 하자
public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyPrefab;
    public int createCount = 10;

    [SerializeField] private float width;
    [SerializeField] private float height;
    private List<GameObject> enemys = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(CoCreateEnemy());
    }

    IEnumerator CoCreateEnemy()
    {
        while (true)
        {

            for (int i = 0; i < createCount; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(-width / 2, width / 2), Random.Range(0, height), transform.position.z), Quaternion.Euler(0, -180, 0));
                enemy.transform.SetParent(transform);
                enemy.GetComponent<Enemy>()?.SetPlayer(player);
                //enemys.Add(enemy);
            }
            yield return new WaitForSeconds(3f);
        }
    }

    private void Init()
    {
        CreateEnemy();
    }

    void CreateEnemy()
    {
        for (int i = 0; i < createCount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(-width / 2, width / 2), Random.Range(0, height), transform.position.z), Quaternion.Euler(0, -180, 0));
            enemy.transform.SetParent(transform);
            enemy.GetComponent<Enemy>()?.SetPlayer(player);
            //enemys.Add(enemy);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));

    }
}
