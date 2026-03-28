using UnityEngine;

using System.Collections;



public class EyeFright : MonoBehaviour

{

    [Header("Setari Timp")]

    public float waitBeforeDisappearing = 1.5f; 

    public float fadeDuration = 1.0f;            

    public float timeUntilReappear = 5f;        



    private bool isTriggered = false;

    private MeshRenderer[] childRenderers;

    private Material[] eyeMaterials;

    private Color[] originalColors;



    void Awake()

    {

        childRenderers = GetComponentsInChildren<MeshRenderer>();

        eyeMaterials = new Material[childRenderers.Length];

        originalColors = new Color[childRenderers.Length];



        for (int i = 0; i < childRenderers.Length; i++)

        {

            // Cream o instanta unica a materialului ca sa nu-i influentam pe ceilalti ochi din scena

            eyeMaterials[i] = childRenderers[i].material;

            originalColors[i] = eyeMaterials[i].color;

        }

    }



    private void OnTriggerEnter(Collider other)

    {

        if (other.CompareTag("Player") && !isTriggered)

        {

            StartCoroutine(DisappearAndReturnWithFade());

        }

    }



    IEnumerator DisappearAndReturnWithFade()

    {

        isTriggered = true;



        yield return new WaitForSeconds(waitBeforeDisappearing);

        

        // --- FADE OUT ---

        SetMaterialToFade(); // Pregatim materialul pentru transparenta

        yield return StartCoroutine(FadeRoutine(0f)); 

        ToggleRenderers(false);



        yield return new WaitForSeconds(timeUntilReappear);

        

        // --- FADE IN ---

        ToggleRenderers(true);

        yield return StartCoroutine(FadeRoutine(1f));

        SetMaterialToOpaque(); // Revenim la modul opac (calitate maxima)



        isTriggered = false;

    }



    IEnumerator FadeRoutine(float targetAlpha)

    {

        float elapsedTime = 0f;

        float startAlpha = targetAlpha == 0f ? 1f : 0f;



        while (elapsedTime < fadeDuration)

        {

            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);



            foreach (Material mat in eyeMaterials)

            {

                if (mat != null)

                {

                    Color c = mat.color;

                    c.a = alpha;

                    mat.color = c;

                }

            }

            yield return null;

        }

    }



    void ToggleRenderers(bool state)

    {

        foreach (var r in childRenderers) if (r != null) r.enabled = state;

    }



    // --- LOGICA TEHNICA PENTRU MATERIALE ---



    void SetMaterialToFade()

    {

        foreach (Material mat in eyeMaterials)

        {

            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);

            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

            mat.SetInt("_ZWrite", 0);

            mat.DisableKeyword("_ALPHATEST_ON");

            mat.EnableKeyword("_ALPHABLEND_ON");

            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");

            mat.renderQueue = 3000;

        }

    }



    void SetMaterialToOpaque()

    {

        foreach (Material mat in eyeMaterials)

        {

            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);

            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);

            mat.SetInt("_ZWrite", 1);

            mat.DisableKeyword("_ALPHATEST_ON");

            mat.DisableKeyword("_ALPHABLEND_ON");

            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");

            mat.renderQueue = -1;

        }

    }

}
