using System;
using System.Collections.Generic;
using UnityEngine;
public class MyObjectPool : MonoBehaviour
{
    private static MyObjectPool _instance;
    public static MyObjectPool Instance
    {
        get { return _instance; }
    }
    //声明字典类用于存储池对象
    Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();
    Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();
    [Serializable]
    public class PoolConfig
    {
        public int count;

        public GameObject prefab;
    }

    public PoolConfig[] poolconfigs;
    void Start()
    {
        _instance = this;
        //根据配置文件，初始化池子，并创建对象
        for (int i = 0; i < this.poolconfigs.Length; i++)
        {
            CreatePool(this.poolconfigs[i].prefab, this.poolconfigs[i].count);
        }
    }
    //创建对象池，并预先存储一些需要实例化的对象
    public static void CreatePool(GameObject prefab, int initialPoolSize)
    {
        if (prefab != null && !Instance.pooledObjects.ContainsKey(prefab))
        {
            var list = new List<GameObject>();
            Instance.pooledObjects.Add(prefab, list);
            if (initialPoolSize > 0)
            {
                bool active = prefab.activeSelf;
                Transform parent = Instance.transform;
                while (list.Count < initialPoolSize)
                {
                    var obj = GameObject.Instantiate(prefab);
                    obj.transform.parent = parent;
                    list.Add(obj);
                }
                prefab.SetActive(false);
            }
        }
    }
    //从对象池中取出对象
    public static GameObject Spawn(GameObject prefab, Transform parnet, Vector3 position, Quaternion rotation)
    {
        List<GameObject> list;
        Transform trans;
        GameObject obj;
        if (Instance.pooledObjects.TryGetValue(prefab, out list))
        {
            obj = null;
            if (list.Count > 0)
            {
                while (obj == null && list.Count > 0)
                {
                    obj = list[0];
                    list.RemoveAt(0);
                }
                if (obj != null)
                {
                    trans = obj.transform;
                    trans.parent = parnet;
                    trans.localPosition = position;
                    trans.localRotation = rotation;
                    obj.SetActive(true);
                    Instance.spawnedObjects.Add(obj, prefab);
                    return obj;
                }
            }
            obj = GameObject.Instantiate(prefab);
            trans = obj.transform;
            trans.parent = parnet;
            trans.localPosition = position;
            trans.localRotation = rotation;
            obj.SetActive(true);
            Instance.spawnedObjects.Add(obj, prefab);
            return obj;
        }
        else
        {
            obj = GameObject.Instantiate(prefab);
            trans = obj.GetComponent<Transform>();
            trans.parent = parnet;
            trans.localPosition = position;
            trans.localRotation = rotation;
            return obj;
        }
    }
    //回收对象
    public static void Recyle(GameObject obj, Action RecyleAction)
    {
        GameObject prefab;
        if (Instance.spawnedObjects.TryGetValue(obj, out prefab))
        {
           // Recyle(obj, prefab);
            Instance.pooledObjects[prefab].Add(obj);
            Instance.spawnedObjects.Remove(obj);
            obj.transform.parent = Instance.transform;
            if (RecyleAction != null)
            {
                RecyleAction();
            }
        }
        else
        {
            Debug.Log("没有找到，要把对象销毁");
            Destroy(obj);
        }
    }

    //static void Recyle(GameObject obj, GameObject prefab)
    //{
       
    //    obj.transform.parent = Instance.transform;
    //    obj.SetActive(false);
    //}
}