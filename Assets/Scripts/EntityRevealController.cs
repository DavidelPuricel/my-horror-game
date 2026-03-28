using System.Collections;
using UnityEngine;

public class EntityRevealController : MonoBehaviour
{
    [Header("Referințe Obiecte")]
    [SerializeField] private GameObject physicalEntity;
    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private Camera cutsceneCamera;

    [Header("Componente Player de oprit")]
    [SerializeField] private MonoBehaviour playerMovement; // Scriptul de mers
    [SerializeField] private PlayerCam playerCamScript;     // Scriptul tău de mouse

    public void StartReveal()
    {
        StartCoroutine(RevealRoutine());
    }

    private IEnumerator RevealRoutine()
    {
        // 1. Înghețăm jucătorul (mers + privire)
        if (playerMovement != null) playerMovement.enabled = false;
        if (playerCamScript != null) playerCamScript.enabled = false;

        // 2. Schimbăm camera
        if (gameplayCamera != null) gameplayCamera.gameObject.SetActive(false);
        if (cutsceneCamera != null) cutsceneCamera.gameObject.SetActive(true);

        // 3. Activăm entitatea
        if (physicalEntity != null) physicalEntity.SetActive(true);

        yield return new WaitForSeconds(3f); // Durata cutscene

        // 4. Revenim la normal
        if (cutsceneCamera != null) cutsceneCamera.gameObject.SetActive(false);
        if (gameplayCamera != null) gameplayCamera.gameObject.SetActive(true);

        // 5. Redăm controlul
        if (playerMovement != null) playerMovement.enabled = true;
        if (playerCamScript != null) playerCamScript.enabled = true;
    }
}
