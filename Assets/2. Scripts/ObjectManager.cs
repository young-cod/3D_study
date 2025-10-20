using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public enum OBJECT { Bullet, Enemy }

    private static ObjectManager instance;
    public static ObjectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectManager>();
            }

            return instance;
        }
    }

    //초기 풀 세팅
    [SerializeField] private ObjectData[] poolSetting;

    //실질적인 풀
    private Dictionary<OBJECT, ObjectData> dicPool = new Dictionary<OBJECT, ObjectData>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetInitiailize();
    }

    private void SetInitiailize()
    {
        foreach (var data in poolSetting)
        {
            //해당 풀이 없다면
            if (!dicPool.ContainsKey(data.type))
            {
                dicPool.Add(data.type, data);
            }

            //풀 초기화
            for (int i = 0; i < data.initialCount; i++)
            {
                GameObject newObj = Instantiate(data.prefab, data.transform);
                newObj.SetActive(false);
                newObj.transform.SetParent(data.transform);
                data.pool.Enqueue(newObj);

                PoolableObject poolable = newObj.AddComponent<PoolableObject>();
                poolable.type = data.type;
            }
        }
    }

    public GameObject GetObject(OBJECT type)
    {
        if (!dicPool.TryGetValue(type, out ObjectData data))
        {
            Debug.LogError($"{type} 해당 타입의 풀이 없습니다!");
        }

        GameObject obj = null;
        Queue<GameObject> pool = data.pool;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            Debug.LogError($"{type} 해당 타입의 풀이 비었습니다."); ;
            //todo : 풀 확장을 할 것인지? 고민 좀
            //obj = Instantiate(data.prefab, data.transform);
        }

        //포지션, 로테이션 설정
        //obj.transform.position = ???
        //obj.transform.rotation = ???

        obj.SetActive(true);
        obj.transform.SetParent(null);

        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);

        PoolableObject poolable = obj.GetComponent<PoolableObject>();

        if (poolable == null)
        {
            Debug.LogError($"{obj.name} 풀 관리 대상이 아닙니다.");
            return;
        }

        //해당 타입의 풀에 반환
        if (dicPool.TryGetValue(poolable.type, out ObjectData data))
        {
            obj.transform.SetParent(data.transform);
            data.pool.Enqueue(obj);
        }else{
            Debug.LogError($"{poolable.type} 해당 타입의 풀이 없습니다. poolSetting을 통해 설정해주세요.");
        }
    }

}
