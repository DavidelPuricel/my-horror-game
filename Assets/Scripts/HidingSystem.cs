using UnityEngine;
using UnityEngine.Playables;

public class HidingSystem : MonoBehaviour
{
    public float interactDistance = 4f; 
    public LayerMask hidingLayer;        

    [Header("Timelines")]
    public PlayableDirector hideTimeline;
    public PlayableDirector deathTimeline; // Timeline-ul de moarte

    [Header("Camere")]
    public GameObject playerMainCamera; 
    public GameObject hideCutsceneCamera; 
    public GameObject deathCamera;        // Camera pentru jumpscare

    [Header("Referinte Jucator")]
    public Transform anchorUnderBed;      
    public Transform exitPoint;           
    public MonoBehaviour playerMovement;  
    public PlayerCam mouseLook; 

    private bool isHidden = false;
    private bool isDead = false; // Flag pentru a preveni activarea multiplă
    private CharacterController controller;

    void Start() {
        controller = GetComponent<CharacterController>();
        if(hideCutsceneCamera != null) hideCutsceneCamera.SetActive(false);
        if(deathCamera != null) deathCamera.SetActive(false);
    }

    void Update() {
        if (isDead) return; // Dacă ești mort, nu mai poți apăsa taste

        if (Input.GetKeyDown(KeyCode.E)) {
            if (!isHidden) {
                CheckForBed();
            } else {
                ExitBed();
            }
        }
    }

    void CheckForBed() {
    RaycastHit hit;
    if (Physics.Raycast(playerMainCamera.transform.position, playerMainCamera.transform.forward, out hit, interactDistance, hidingLayer)) {
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("HidingSpot")) {
            
            // --- VERIFICARE DISTANȚĂ MĂCELAR ---
            GameObject butcherAI = GameObject.FindGameObjectWithTag("ButcherAI");
            
            if (butcherAI != null) {
                float distanceToButcher = Vector3.Distance(transform.position, butcherAI.transform.position);
                
                if (distanceToButcher < 5f) { // Dacă e la mai puțin de 5 unități
                    Debug.Log("Măcelarul te-a văzut intrând! Te prinde!");
                    TriggerCaughtDeath(); // SARIM DIRECT LA MOARTE
                } else {
                    StartHiding(); // Ești destul de departe, te poți ascunde
                }
            } else {
                // Dacă măcelarul nu e activ în scenă (ex: la începutul jocului), te ascunzi normal
                StartHiding();
            }
        }
    }
}

    void StartHiding() {
        if (hideCutsceneCamera == null) return;

        playerMovement.enabled = false;
        hideCutsceneCamera.SetActive(true);
        playerMainCamera.SetActive(false);

        hideTimeline.Play();
        hideTimeline.stopped += OnHideFinished;
    }

    void OnHideFinished(PlayableDirector director) {
        if (controller != null) {
            controller.enabled = false;
            controller.height = 0.5f; 
            controller.center = new Vector3(0, 0.25f, 0); 
        }

        transform.position = anchorUnderBed.position;
        transform.rotation = anchorUnderBed.rotation;
        playerMainCamera.transform.localPosition = new Vector3(0, 0.2f, 0);

        Physics.SyncTransforms();

        hideCutsceneCamera.SetActive(false);
        playerMainCamera.SetActive(true);

        if (controller != null) controller.enabled = true;

        if (mouseLook != null) {
            mouseLook.enabled = true;
            mouseLook.isHiding = true;
            mouseLook.ForceRotation(0f, anchorUnderBed.eulerAngles.y);
        }

        isHidden = true;
        hideTimeline.stopped -= OnHideFinished;
    }

    void ExitBed() {
        if (controller != null) controller.enabled = false;

        controller.height = 2.0f; 
        controller.center = new Vector3(0, 1.0f, 0); 

        playerMainCamera.transform.localPosition = new Vector3(0, 1.43f, 0);
        playerMainCamera.transform.localRotation = Quaternion.identity;

        transform.localScale = Vector3.one;

        if (exitPoint != null) {
            transform.position = exitPoint.position;
            transform.rotation = exitPoint.rotation;
        }

        if (mouseLook != null) {
            mouseLook.isHiding = false;
            mouseLook.ForceRotation(0f, exitPoint.eulerAngles.y);
        }

        Physics.SyncTransforms();

        if (controller != null) controller.enabled = true;
        if (playerMovement != null) playerMovement.enabled = true;

        isHidden = false;
        Debug.Log("Jucătorul a ieșit.");
    }

    // --- LOGICA DE MOARTE (JUMPSCARE) ---

    [ContextMenu("Trigger Death Test")] // Permite testarea din Inspector (click dreapta pe script)
    public void TriggerCaughtDeath() {
        if (isDead) return;
        isDead = true;
        isHidden = false;

        // 1. Dezactivăm controlul jucătorului
        if (controller != null) controller.enabled = false;
        if (playerMovement != null) playerMovement.enabled = false;
        if (mouseLook != null) mouseLook.enabled = false;

        // 2. Schimbăm pe camera de cinematică
        playerMainCamera.SetActive(false);
        if (deathCamera != null) deathCamera.SetActive(true);

        // 3. Dezactivăm Măcelarul "AI" (cel care bântuie) ca să nu se bată cu Timeline-ul
        GameObject butcherAI = GameObject.FindGameObjectWithTag("ButcherAI");
        if (butcherAI != null) butcherAI.SetActive(false);

        // 4. Pornim Timeline-ul de jumpscare
        if (deathTimeline != null) {
            deathTimeline.Play();
        }

        Debug.Log("Jucătorul a fost prins!");
    }
}
