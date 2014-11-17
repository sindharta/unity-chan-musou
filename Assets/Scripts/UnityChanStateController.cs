using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityChanAnimationController))]
public class UnityChanStateController : MonoBehaviour {
    
    [SerializeField]
    float m_runSpeed = 5.0f;
    [SerializeField]
    float m_walkSpeed = 1.0f;
    
//----------------------------------------------------------------------------------------------------------------------    

    enum EUnityChanState {
        IDLE = 0,
        WALK,
        RUN,
        MAX,
    };


    UnityChanAnimationController m_animationController = null;
    EUnityChanState m_state;
    Transform m_transform = null;
    
    float m_speed = 0;
    
//----------------------------------------------------------------------------------------------------------------------    

    void Awake() {
        m_animationController = GetComponent<UnityChanAnimationController>();
        m_transform = GetComponent<Transform>();
    }
    
//----------------------------------------------------------------------------------------------------------------------    

	// Use this for initialization
	void Start () {
        SetState (EUnityChanState.IDLE);        
	}
	
//----------------------------------------------------------------------------------------------------------------------    

    // Update is called once per frame
	void Update () {
        UpdatePos();
    
        if (m_animationController.IsPlaying())
            return;
            
        SetState ((EUnityChanState) Random.Range(0, (int) EUnityChanState.MAX));
	
	}

//----------------------------------------------------------------------------------------------------------------------        

    void UpdatePos() {
        m_transform.localPosition = m_transform.localPosition + m_transform.forward * m_speed * Time.deltaTime;
    }
        
//----------------------------------------------------------------------------------------------------------------------        

    void SetState(EUnityChanState state) {
        m_state = state;
        switch(m_state) {
            case EUnityChanState.IDLE : {
                m_animationController.PlayIdleAnimation();
                m_speed = 0;
                break;
            }
            case EUnityChanState.WALK : {            
                m_animationController.PlayWalkAnimation();
                m_speed = m_walkSpeed;
                break;
            }
            case EUnityChanState.RUN : {            
                m_animationController.PlayRunAnimation();
                m_speed = m_runSpeed;
                break;
            }
        }
        
        //randomize rotation
        float y_rot = Random.Range(0f, 360.0f);
        m_transform.Rotate(new Vector3(0,y_rot,0));
       
    }


//----------------------------------------------------------------------------------------------------------------------        
    
}
