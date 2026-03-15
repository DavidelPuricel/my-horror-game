using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    public Door[] doors;

    public void Interact()
    {
        Debug.Log("Switch pressed!");

        foreach (Door door in doors)
        {
            if (door != null)
            {
                door.Toggle();
            }
        }
    }

    void Start()
    {
    }

    void Update()
    {
    }
}
