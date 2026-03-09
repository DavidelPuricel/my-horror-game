using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable

    
{
    public Door door;
    public void Interact() 
    {
        Debug.Log("Switch pressed!");
        door.Toggle();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
