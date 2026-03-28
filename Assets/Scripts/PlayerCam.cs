using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;
    
    public Transform orientation;
    
    float xRotation;
    float yRotation;

    [HideInInspector] public bool isHiding = false; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;

        if (isHiding)
        {
            // Limite sub pat: nu te uiti prea sus sau prea jos
            xRotation = Mathf.Clamp(xRotation, -30f, 30f); 
        }
        else
        {
            // Limita normala in picioare
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        }
        
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    // Aceasta functie "fura" controlul de la mouse si il pune pe pozitia ancorei
    public void ForceRotation(float x, float y)
    {
        xRotation = x;
        yRotation = y;
    }
}
