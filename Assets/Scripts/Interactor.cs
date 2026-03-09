using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}


public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange;
    public LayerMask interactMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            Debug.DrawRay(InteractorSource.position, InteractorSource.forward * InteractRange, Color.red, 1f);
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange, interactMask))
            {
                Debug.Log("Hit: " + hitInfo.collider.name);

                if (hitInfo.collider.TryGetComponent<IInteractable>(out var interactObj))
                {
                    Debug.Log("Found IInteractable!");
                    interactObj.Interact();
                }
            }
        }
    }
}
