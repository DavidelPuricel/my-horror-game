using System.Collections;
using UnityEngine;

public class Statue : MonoBehaviour, IInteractable
{
    public Statue targetStatue;
    public float rotateDuration = 0.5f;

    private int currentRotation = 0;
    private bool isRotating = false;
    private Coroutine rotateRoutine;

    public void Interact()
    {
        if (isRotating) return;

        currentRotation = (currentRotation + 1) % 4;

        Quaternion targetRotation = transform.localRotation * Quaternion.Euler(0f, 90f, 0f);

        if (rotateRoutine != null)
        {
            StopCoroutine(rotateRoutine);
        }

        rotateRoutine = StartCoroutine(RotateStatue(targetRotation));
    }

    private IEnumerator RotateStatue(Quaternion targetRotation)
    {
        isRotating = true;

        Quaternion startRotation = transform.localRotation;
        float elapsed = 0f;
        float duration = Mathf.Max(0.01f, rotateDuration);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.localRotation = targetRotation;
        isRotating = false;
        rotateRoutine = null;

        StatuePuzzleManager.instance.CheckPuzzle();
    }

    public bool IsFacingTarget()
    {
        if (targetStatue == null) return false;

        Vector3 direction = (targetStatue.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, direction);

        return dot > 0.9f;
    }
}
