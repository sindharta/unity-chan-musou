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
            
        if (Time.deltaTime <= m_idealDeltaTime)  {
            float x = Random.Range(-m_spawnAreaHalfSize, m_spawnAreaHalfSize);
            float z = Random.Range(-m_spawnAreaHalfSize, m_spawnAreaHalfSize);

            float y_rot = Random.Range(0f, 360.0f);            
            
            GameObject obj = (GameObject) GameObject.Instantiate(
                m_unityChanPrefab, 
                new Vector3(x,0,z), 
                Quaternion.Euler(0,y_rot,0)
            );
            obj.transform.parent = m_unityChanRoot;
            m_unityChanObjects.Push(obj);
        } else {
            if (m_unityChanObjects.Count > 0) {
                GameObject obj = m_unityChanObjects.Pop();
                Destroy(obj);
            }           
        }                     
            
        m_lastUpdateTime = Time.realtimeSinceStartup;
	}
    
//----------------------------------------------------------------------------------------------------------------------
    void OnGUI()  {

        GUI.Label(new Rect(10, 10, 150, 20), "Delta Time: " + Time.deltaTime.ToString());
        GUI.Label(new Rect(10, 30, 150, 20), "Objects count: " + m_unityChanObjects.Count.ToString());
    }
    
        
}
