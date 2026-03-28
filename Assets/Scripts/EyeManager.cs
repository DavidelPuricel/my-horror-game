using UnityEngine;
using System.Collections.Generic;

public class EyesManager : MonoBehaviour
{
    [Tooltip("Trage aici toti ochii de pe pereti din Hierarchy")]
    public List<GameObject> allEyesInHouse;

    public void RevealAllEyes()
{
    foreach (GameObject eye in allEyesInHouse)
    {
        if (eye != null) 
        {
            eye.SetActive(true); // Aici se aprind ochii!
            Debug.Log("Am activat ochiul: " + eye.name);
        }
    }
}

    public void HideAllEyes()
    {
        foreach (GameObject eye in allEyesInHouse)
        {
            if (eye != null) eye.SetActive(false);
        }
    }
}
