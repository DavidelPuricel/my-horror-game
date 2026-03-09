using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 openOffset = new Vector3(0f, 3f, 0f);
    [SerializeField] private float moveDuration = 0.6f;

    private bool isOpen;
    private Vector3 openPosition;
    private Vector3 closedPosition;
    private Coroutine moveRoutine;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;   
        isOpen = false;
    }

    public void Toggle()
    {
        Debug.Log("Door toggled!");

        Vector3 targetPos = isOpen ? closedPosition : openPosition;


        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
            moveRoutine = null;
        }

        moveRoutine = StartCoroutine(MoveDoor(targetPos));


        isOpen = !isOpen;
    }

    private IEnumerator MoveDoor(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        float t = 0f;

        float duration = Mathf.Max(0.01f, moveDuration);

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
        moveRoutine = null;
    }
}
