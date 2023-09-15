using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScreenShake : MonoBehaviour
{
    public bool isShaking = false;
    public float duration = 1f;
    public AnimationCurve animationCurve;

    private void FixedUpdate()
    {
        if (isShaking)
        {
            isShaking = false;
            StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake()
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.smoothDeltaTime;
            float strength = animationCurve.Evaluate(elapsedTime / duration);
            transform.position = new Vector3(startPos.x + Random.insideUnitSphere.x * strength,
                startPos.y + Random.insideUnitSphere.y * strength, transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(startPos.x, startPos.y, transform.position.z);
    }
}
