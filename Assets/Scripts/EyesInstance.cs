using System.Collections;
using UnityEngine;

public class EyesInstance : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private float fadeInDuration = 0.4f;
    [SerializeField] private float visibleDuration = 1.5f;
    [SerializeField] private float fadeOutDuration = 0.4f;

    private Material[] materials;

    private void Awake()
    {
        materials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
            SetAlpha(materials[i], 0f);
        }
    }

    public IEnumerator PlayNormalSequence()
    {
        yield return Fade(0f, 1f, fadeInDuration);
        yield return new WaitForSeconds(visibleDuration);
        yield return Fade(1f, 0f, fadeOutDuration);
    }

    public void ShowFinalEyes()
    {
        StopAllCoroutines();
        StartCoroutine(Fade(0f, 1f, fadeInDuration));
    }

    public IEnumerator FadeOutAndDestroy(float duration)
    {
        yield return Fade(1f, 0f, duration);
    }

    private IEnumerator Fade(float start, float end, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, t / duration);

            foreach (Material mat in materials)
                SetAlpha(mat, alpha);

            yield return null;
        }

        foreach (Material mat in materials)
            SetAlpha(mat, end);
    }

    private void SetAlpha(Material mat, float alpha)
    {
        if (mat.HasProperty("_Color"))
        {
            Color c = mat.color;
            c.a = alpha;
            mat.color = c;
        }
    }

    public void SetGlowStrength(float intensity)
    {
        foreach (Material mat in materials)
        {
            if (mat.HasProperty("_EmissionColor"))
            {
                Color emission = Color.white * intensity;
                mat.SetColor("_EmissionColor", emission);
            }
        }
    }
}
