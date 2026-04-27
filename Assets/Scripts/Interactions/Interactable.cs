using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

public abstract class Interactable : MonoBehaviour { 
    
    //puts interactable objects on level 6 
    public virtual void Awake() { gameObject.layer = 6; } //when player interacts 
    
    public abstract void OnInteract(); //when looking at object 
    
    public abstract void OnFocus(); //when looking away from object 
    public abstract void OnLoseFocus(); 
    
}