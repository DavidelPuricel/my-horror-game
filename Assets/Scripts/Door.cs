using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float rotateDuration = 0.6f;

    private bool isOpen;
    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine rotateRoutine;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);
        isOpen = false;
    }

    public void Toggle()
    {
        Quaternion targetRotation = isOpen ? closedRotation : openRotation;

        if (rotateRoutine != null)
        {
            StopCoroutine(rotateRoutine);
            rotateRoutine = null;
        }

        rotateRoutine = StartCoroutine(RotateDoor(targetRotation));
        isOpen = !isOpen;
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        Quaternion startRotation = transform.localRotation;
        float t = 0f;

        float duration = Mathf.Max(0.01f, rotateDuration);

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.localRotation = targetRotation;
        rotateRoutine = null;
    }
}
