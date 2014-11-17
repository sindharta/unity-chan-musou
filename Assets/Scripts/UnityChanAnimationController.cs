using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animation))]
public class UnityChanAnimationController : MonoBehaviour {

    [SerializeField]
    List<AnimationClip> m_waitAnimationClips;
    
    [SerializeField]
    AnimationClip m_runAnimationClip;
    
    [SerializeField]
    AnimationClip m_walkAnimationClip;
    
//----------------------------------------------------------------------------------------------------------------------

	Animation m_animation;

//----------------------------------------------------------------------------------------------------------------------

    void Awake() {
        m_animation = GetComponent<Animation>();
        
        //add all animations
        m_animation.AddClip(m_runAnimationClip, m_runAnimationClip.name);        
        m_animation.AddClip(m_walkAnimationClip, m_walkAnimationClip.name);        
        
        int count = m_waitAnimationClips.Count;
        for (int i=0;i<count;++i) {
            m_animation.AddClip(m_waitAnimationClips[i], m_waitAnimationClips[i].name);
        }
    }
//----------------------------------------------------------------------------------------------------------------------    
    
    public void PlayRunAnimation() {
        m_animation.CrossFade(m_runAnimationClip.name);
    }
       
//----------------------------------------------------------------------------------------------------------------------	
    
    public void PlayWalkAnimation() {
        m_animation.CrossFade(m_walkAnimationClip.name);
    }    
    
//----------------------------------------------------------------------------------------------------------------------    

    public void PlayIdleAnimation() {
        int index = Random.Range(0,m_waitAnimationClips.Count);
        m_animation.CrossFade(m_waitAnimationClips[index].name);
    }

//----------------------------------------------------------------------------------------------------------------------        

    public bool IsPlaying() {
        return m_animation.isPlaying;
    }
}
