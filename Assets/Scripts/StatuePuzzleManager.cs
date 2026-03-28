using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatuePuzzleManager : MonoBehaviour
{
public static StatuePuzzleManager instance;
public Statue[] statues;
public Door door;
void Awake()
{
	instance = this;
}
public void CheckPuzzle()
{
    Debug.Log("Checking puzzle");

    foreach (Statue statue in statues)
    {
        if (!statue.IsFacingTarget())
        {
            Debug.Log("Puzzle not solved");
            return;
        }
    }

    Debug.Log("Puzzle solved! Opening door...");
    door.Toggle();
}
    
}
