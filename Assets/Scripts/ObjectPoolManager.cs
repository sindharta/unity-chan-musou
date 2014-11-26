using UnityEngine;
using System.Collections.Generic;

//Obtained and modified from http://unitypatterns.com/resource/objectpool/

public sealed class ObjectPoolManager : MonoBehaviour
{
    public enum StartupPoolMode { Awake, Start, CallManually };
    
    [System.Serializable]
    public class StartupPool
    {
        public int size;
        public GameObject prefab;
    }

    [SerializeField]
    StartupPoolMode m_startupPoolMode;
    [SerializeField]
    StartupPool[] m_startupPools;
                
    static ObjectPoolManager m_instance;
    
    //prefab, List<unused gameobject>
    Dictionary<GameObject, Stack<GameObject>> m_pooledObjects = new Dictionary<GameObject, Stack<GameObject>>();
    
    //Gameobject, and then the prefab
    Dictionary<GameObject, GameObject> m_spawnedObjects = new Dictionary<GameObject, GameObject>();
        
    bool m_startupPoolsCreated;

//----------------------------------------------------------------------------------------------------------------------
    public static ObjectPoolManager GetInstance() {
        if (m_instance != null)
            return m_instance;
        
        m_instance = Object.FindObjectOfType<ObjectPoolManager>();
        if (m_instance != null)
            return m_instance;
        
        GameObject obj = new GameObject("ObjectPool");
        Transform t = obj.transform;
        t.position = Vector3.zero;
        t.rotation = Quaternion.identity;
        t.localScale = Vector3.one;
        m_instance = obj.AddComponent<ObjectPoolManager>();
        return m_instance;
    }

//----------------------------------------------------------------------------------------------------------------------
        
    void Awake() {
        m_instance = this;
        if (m_startupPoolMode == StartupPoolMode.Awake)
            CreateStartupPools();
    }

//----------------------------------------------------------------------------------------------------------------------

    void Start() {
        if (m_startupPoolMode == StartupPoolMode.Start)
            CreateStartupPools();
    }
    
//----------------------------------------------------------------------------------------------------------------------
    void CreateStartupPools() {
        StartupPool[] pools = m_instance.m_startupPools;
        if (pools != null && pools.Length > 0) {
            for (int i = 0; i < pools.Length; ++i)
                CreatePool(pools[i].prefab, pools[i].size);
        }
    }
    
//----------------------------------------------------------------------------------------------------------------------
    public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component  {
        CreatePool(prefab.gameObject, initialPoolSize);
    }
    
//----------------------------------------------------------------------------------------------------------------------    
    public static void CreatePool(GameObject prefab, int initialPoolSize)    {
        ObjectPoolManager manager = GetInstance();
        if (!manager.m_pooledObjects.ContainsKey(prefab)) {
            Stack<GameObject> stack = new Stack<GameObject>(initialPoolSize);
            manager.m_pooledObjects.Add(prefab, stack);
            
            if (initialPoolSize <=0)
                return;
                
            bool active = prefab.activeSelf;
            prefab.SetActive(false);
            Transform parent = manager.transform;
            for (int i = 0;i< initialPoolSize;++i) {
                GameObject obj = (GameObject)Object.Instantiate(prefab);
                obj.transform.parent = parent;
                stack.Push(obj);
            }
            prefab.SetActive(active);
        }
    }
    
//----------------------------------------------------------------------------------------------------------------------
    #region Spawn
    public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component  {
        return Spawn(prefab.gameObject, parent, position, rotation).GetComponent<T>();
    }
    public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component  {
        return Spawn(prefab.gameObject, null, position, rotation).GetComponent<T>();
    }
    public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component  {
        return Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
    }
    public static T Spawn<T>(T prefab, Vector3 position) where T : Component {
        return Spawn(prefab.gameObject, null, position, Quaternion.identity).GetComponent<T>();
    }
    public static T Spawn<T>(T prefab, Transform parent) where T : Component  {
        return Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
    }
    public static T Spawn<T>(T prefab) where T : Component {
        return Spawn(prefab.gameObject, null, Vector3.zero, Quaternion.identity).GetComponent<T>();
    }
    
    public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position) {
        return Spawn(prefab, parent, position, Quaternion.identity);
    }
    
    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation) {
        return Spawn(prefab, null, position, rotation);
    }
    
    public static GameObject Spawn(GameObject prefab, Transform parent)  {
        return Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
    }
    
    public static GameObject Spawn(GameObject prefab, Vector3 position) {
        return Spawn(prefab, null, position, Quaternion.identity);
    }
    
    public static GameObject Spawn(GameObject prefab)  {
        return Spawn(prefab, null, Vector3.zero, Quaternion.identity);
    }

        
    public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)  {

        ObjectPoolManager manager = GetInstance();        
        if (!manager.m_pooledObjects.ContainsKey(prefab)) {
            CreatePool(prefab, 0);
        }
          
        Stack<GameObject> stack = manager.m_pooledObjects[prefab];        
        
        GameObject obj = (stack.Count > 0) ? stack.Pop() : (GameObject)Object.Instantiate(prefab);
        Transform t = obj.transform;

        t.parent   = parent;
        t.position = position;
        t.rotation = rotation;
        obj.SetActive(true);
        
        manager.m_spawnedObjects.Add(obj, prefab);
        return obj;
        
    }
    
    #endregion
    
//----------------------------------------------------------------------------------------------------------------------

    #region Recyle    
    public static void Recycle<T>(T obj) where T : Component {
        Recycle(obj.gameObject);
    }

//----------------------------------------------------------------------------------------------------------------------
    
    public static void Recycle(GameObject obj) {
        GameObject prefab;
        if (GetInstance().m_spawnedObjects.TryGetValue(obj, out prefab))
            Recycle(obj, prefab);
        else {
            Debug.LogWarning("This object wasn't spawned from ObjectPoolManager: " + obj.name);
            Object.Destroy(obj);
        }
    }
    
//----------------------------------------------------------------------------------------------------------------------    
    static void Recycle(GameObject obj, GameObject prefab)  {
        ObjectPoolManager manager = GetInstance();
    
        manager.m_pooledObjects[prefab].Push(obj);
        manager.m_spawnedObjects.Remove(obj);
        obj.transform.parent = manager.transform;
        obj.SetActive(false);
    }

    #endregion
    
//----------------------------------------------------------------------------------------------------------------------
   
    
}

