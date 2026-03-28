using UnityEngine;
using UnityEngine.Playables;

public class EntityPhaseManager : MonoBehaviour
{
    public static EntityPhaseManager Instance;

    [Header("Faza 1: Ochii")]
    public EyesManager eyesMgr; 

    [Header("Faza 2: Cutscene & Camere")]
    public PlayableDirector cutsceneTimeline; 
    public GameObject playerCamera;    // Camera principala a jucatorului
    public GameObject cutsceneCamera;  // Camera folosita pentru animatia de Timeline

    [Header("Faza 3: Gameplay AI")]
    public MonoBehaviour butcherAI;    // Scriptul de urmarire de pe Macelar

    private int skullsCollected = 0;

    void Awake()
    {
        // Ne asiguram ca avem o singura instanta a Managerului
        if (Instance == null) Instance = this;
        
        // La inceputul jocului, AI-ul trebuie sa fie oprit
        if (butcherAI != null) butcherAI.enabled = false;
        
        // Ne asiguram ca si camera de cutscene e oprita la start
        if (cutsceneCamera != null) cutsceneCamera.SetActive(false);
    }

    // Metoda chemata de Collectible.cs
    public void CollectSkull(int skullID) 
    { 
        skullsCollected++;
        Debug.Log("Sistem: Craniu colectat! Total: " + skullsCollected);

        if (skullsCollected == 1)
        {
            // FAZA 1: Apar ochii
            if (eyesMgr != null) 
            {
                eyesMgr.RevealAllEyes(); 
                Debug.Log("Faza 1: Ochii s-au activat.");
            }
        }
        else if (skullsCollected == 2)
        {
            // FAZA 2: Pregatim si pornim Timeline-ul
            PrepareAndStartCutscene();
        }
    }

    private void PrepareAndStartCutscene()
    {
        // 1. Oprim ochii (sa nu "polueze" scena de film)
        if (eyesMgr != null)
        {
            // Daca nu ai metoda HideAllEyes, foloseste: eyesMgr.gameObject.SetActive(false);
            eyesMgr.gameObject.SetActive(false); 
        }

        if (cutsceneTimeline != null)
        {
            // 2. Schimbam camerele: Oprim jucatorul, activam regia
            if (playerCamera != null) playerCamera.SetActive(false);
            if (cutsceneCamera != null) cutsceneCamera.SetActive(true);

            // 3. Ne abonam la evenimentul de final si dam PLAY
            cutsceneTimeline.stopped += OnTimelineFinished;
            cutsceneTimeline.Play();
            
            Debug.Log("Faza 2: Timeline a pornit cu succes!");
        }
        else
        {
            Debug.LogError("EROARE: Nu ai tras Timeline-ul in Inspector pe EntityPhaseManager!");
        }
    }

    private void OnTimelineFinished(PlayableDirector director)
{
    Debug.Log("Sistem: Sincronizare poziție post-cutscene...");

    // 1. Găsim "rădăcina" jucătorului (obiectul părinte care are CharacterController/Movement)
    // playerCamera.transform.root ia obiectul cel mai de sus din ierarhie
    GameObject playerRoot = playerCamera.transform.root.gameObject;

    // 2. TELEPORTAREA: Mutăm corpul jucătorului exact unde a rămas camera de film
    playerRoot.transform.position = cutsceneCamera.transform.position;
    
    // Îi dăm și rotația, ca să te uiți în aceeași direcție în care se uita camera în film
    playerRoot.transform.rotation = cutsceneCamera.transform.rotation;

    // 3. SCHIMBĂM CAMERELE
    if (cutsceneCamera != null) cutsceneCamera.SetActive(false);
    if (playerCamera != null) playerCamera.SetActive(true);

    // 4. ACTIVĂM AI-UL MĂCELARULUI
    if (butcherAI != null)
    {
        // Dacă Măcelarul are NavMeshAgent, trebuie să-i dăm "Warp" 
        // ca să nu încerce să fugă înapoi la poziția inițială
        var agent = butcherAI.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) 
        {
            agent.Warp(butcherAI.transform.position);
        }
        
        butcherAI.enabled = true;
    }

    // Dezabonare pentru a evita rulări multiple
    cutsceneTimeline.stopped -= OnTimelineFinished;
    
    Debug.Log("Sistem: Control redat jucătorului în noua poziție.");
}
}
