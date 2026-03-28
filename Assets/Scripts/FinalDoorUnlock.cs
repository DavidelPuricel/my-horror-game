using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoorUnlock : MonoBehaviour
{
    public Door finalDoor;
    private bool isOpen = false;
    public void Interact()
    {
    	if(isOpen) return;
    	if(SkullManager.instance.hasAllSkull())
    	{
    	Debug.Log("All Skulls Collected!");
    	finalDoor.Toggle();
    	isOpen=true;
    	}
    	else
    	{
    	Debug.Log("You need more skulls");
    	return;
    	}
    }
}
