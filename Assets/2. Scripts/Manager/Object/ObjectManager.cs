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

    //�ʱ� Ǯ ����
    [SerializeField] private ObjectData[] poolSetting;

    //�������� Ǯ
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
            //�ش� Ǯ�� ���ٸ�
            if (!dicPool.ContainsKey(data.type))
            {
                dicPool.Add(data.type, data);
            }

            //Ǯ �ʱ�ȭ
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
            Debug.LogError($"{type} �ش� Ÿ���� Ǯ�� �����ϴ�!");
        }

        GameObject obj = null;
        Queue<GameObject> pool = data.pool;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            Debug.LogError($"{type} �ش� Ÿ���� Ǯ�� ������ϴ�."); ;
            //todo : Ǯ Ȯ���� �� ������? ��� ��
            //obj = Instantiate(data.prefab, data.transform);
        }

        //������, �����̼� ����
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
            Debug.LogError($"{obj.name} Ǯ ���� ����� �ƴմϴ�.");
            return;
        }

        //�ش� Ÿ���� Ǯ�� ��ȯ
        if (dicPool.TryGetValue(poolable.type, out ObjectData data))
        {
            obj.transform.SetParent(data.transform);
            data.pool.Enqueue(obj);
        }else{
            Debug.LogError($"{poolable.type} �ش� Ÿ���� Ǯ�� �����ϴ�. poolSetting�� ���� �������ּ���.");
        }
    }

}
