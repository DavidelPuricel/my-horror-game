using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour 
{
    public PlayableDirector director; // Trage aici obiectul cu Timeline
    public MonoBehaviour playerMovement; // Scriptul de mers
    public MonoBehaviour mouseLook; // Scriptul de rotire camera
    public GameObject macealarAI; // Scriptul EntityAI de pe macelar

    public void PlayCutscene() 
    {
        // 1. Înghețăm input-ul jucătorului
        playerMovement.enabled = false;
        mouseLook.enabled = false;

        // 2. Pornim Timeline-ul
        director.Play();

        // 3. Ne abonăm la finalul Timeline-ului ca să redăm controlul
        director.stopped += OnTimelineFinished;
    }

    void OnTimelineFinished(PlayableDirector obj) 
    {
        // Redăm controlul jucătorului
        playerMovement.enabled = true;
        mouseLook.enabled = true;

        // Activăm AI-ul măcelarului să înceapă vânătoarea
        if(macealarAI != null) macealarAI.GetComponent<EntityAI>().enabled = true;

        director.stopped -= OnTimelineFinished; // Curățăm evenimentul
    }
}
