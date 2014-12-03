using UnityEngine;
using System.Collections.Generic;

public class BakedSkinnedMesh : MonoBehaviour {

    [SerializeField]
    Transform m_sourceSkinnedMesh;
    
    Transform m_cachedTransform;
    
    List<MeshFilter> m_meshFilters;
    List<SkinnedMeshRenderer> m_sourceSkinnedRenderers; //source

//----------------------------------------------------------------------------------------------------------------------
    
    void Awake() {
        m_cachedTransform = GetComponent<Transform>();
        
        m_meshFilters = new List<MeshFilter>(4);
        m_sourceSkinnedRenderers = new List<SkinnedMeshRenderer>(4);        
    }
    
//----------------------------------------------------------------------------------------------------------------------    

	// Use this for initialization
	void Start () {
        SkinnedMeshRenderer[] skinned_renderers = m_sourceSkinnedMesh.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i=0;i<skinned_renderers.Length;++i) {
            GameObject obj = new GameObject();
            Transform t = obj.transform;
            t.parent = m_cachedTransform;
            t.localPosition = Vector3.zero;            

            
            MeshFilter mesh_filter = obj.AddComponent<MeshFilter>();
            mesh_filter.mesh = new Mesh();
            
            m_meshFilters.Add(mesh_filter);
            MeshRenderer baked_mesh_renderer = obj.AddComponent<MeshRenderer>();
            m_sourceSkinnedRenderers.Add(skinned_renderers[i]);
            baked_mesh_renderer.material = skinned_renderers[i].material;
            
        }	    
	}
	
//----------------------------------------------------------------------------------------------------------------------    
	// Update is called once per frame
	void Update () {
        for (int i=0;i<m_meshFilters.Count;++i) {
            m_sourceSkinnedRenderers[i].BakeMesh(m_meshFilters[i].mesh);
        }
	
	}
    
//----------------------------------------------------------------------------------------------------------------------
    
}
