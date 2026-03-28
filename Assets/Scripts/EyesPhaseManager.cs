using UnityEngine;

public class EyesPhaseManager : MonoBehaviour
{
    [Header("Eyes Renderers")]
    public Renderer[] eyeRenderers; // rendererele ochilor

    void Start()
    {
        // Ascundem ochii la start
        SetEyesVisible(false);
    }

    // Apelată de GameManager
    public void RevealEyes()
    {
        SetEyesVisible(true);
        // Poți adăuga aici glow, puls sau efecte suplimentare
    }

    public void HideEyes()
    {
        SetEyesVisible(false);
    }

    private void SetEyesVisible(bool visible)
    {
        if (eyeRenderers == null) return;

        foreach (var r in eyeRenderers)
        {
            r.enabled = visible;
        }
    }
}
