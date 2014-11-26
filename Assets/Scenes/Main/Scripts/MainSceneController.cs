using UnityEngine;
using System.Collections.Generic;

public class MainSceneController : MonoBehaviour {

    [SerializeField]
    GameObject m_unityChanPrefab;
    
    [SerializeField]
    Transform m_unityChanRoot;
    
    [SerializeField]
    float m_updateFrequency = 0.3f;
    
    [SerializeField]
    float m_idealDeltaTime = 1.0f / 30.0f; // 1 / FPS.
    
    [SerializeField]
    float m_spawnAreaHalfSize = 10.0f;
    
    string m_deltaTimeInfo;    
    
//----------------------------------------------------------------------------------------------------------------------            

    float m_lastUpdateTime = 0;
    
    //TODO-Sin: 2014-11-20, use pool.
    Stack<GameObject> m_unityChanObjects;
    

//----------------------------------------------------------------------------------------------------------------------
	// Use this for initialization
	void Awake () {	
        m_lastUpdateTime = 0;
        m_unityChanObjects = new Stack<GameObject>();
	}
	
//----------------------------------------------------------------------------------------------------------------------

    // Update is called once per frame
	void Update () {

        if (Time.realtimeSinceStartup - m_lastUpdateTime < m_updateFrequency)
            return;
            
        //We can still add more 
        if (Time.deltaTime <= m_idealDeltaTime)  {
            float x = Random.Range(-m_spawnAreaHalfSize, m_spawnAreaHalfSize);
            float z = Random.Range(-m_spawnAreaHalfSize, m_spawnAreaHalfSize);

            float y_rot = Random.Range(0f, 360.0f);            
            
            Vector3 pos = new Vector3(x,0,z);
            Quaternion rot = Quaternion.Euler(0,y_rot,0);
            
            GameObject obj = ObjectPoolManager.Spawn(m_unityChanPrefab, m_unityChanRoot, pos, rot);
            
            m_unityChanObjects.Push(obj);
        } else {
            //we have to decrease to maintain the ideal delta time
            if (m_unityChanObjects.Count > 0) {
                ObjectPoolManager.Recycle(m_unityChanObjects.Pop());
            }           
        }                     
            
        m_deltaTimeInfo = string.Format("Delta Time: {0:0.##} ({1:00} FPS). Ideal: {2:0.##} ({3:00} FPS)", 
                                               Time.deltaTime, 
                                               1.0f / Time.deltaTime, 
                                               m_idealDeltaTime, 
                                               1.0f / m_idealDeltaTime);
        m_lastUpdateTime = Time.realtimeSinceStartup;
	}
    
//----------------------------------------------------------------------------------------------------------------------
    void OnGUI()  {
        GUI.Label(new Rect(10, 10, 450, 20), m_deltaTimeInfo);
        GUI.Label(new Rect(10, 30, 150, 20), "Objects count: " + m_unityChanObjects.Count.ToString());
    }
    
        
}
