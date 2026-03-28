using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullManager : MonoBehaviour
{
	public static SkullManager instance;
	private int skulls = 0;
	[SerializeField] private int skullsNedeed=5;
	[SerializeField] private Door finalDoor;
	private bool finalDoorOpen = false;
	void Awake ()
	{
	instance = this;
	}
	public void AddSkull()
	{
	skulls++;
	Debug.Log("Skulls: " + skulls + "/" + skullsNedeed);
	if(!finalDoorOpen && skulls>=skullsNedeed)
	{
	Debug.Log("All skulls collected. Opening final door...");
	if(finalDoor != null)
	{
	finalDoor.Toggle();
	finalDoorOpen = true;
	}
	else
	{
	Debug.Log("Final door is not assigned in SkullManager");
	}
	
	}
	
	}
	public bool hasAllSkull()
	{
	return skulls>=skullsNedeed;
	}
  
}
