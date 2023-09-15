using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SmoothCamera : MonoBehaviour
{
    public Vector3 offset;
    [SerializeField] private Transform player;
    [SerializeField] private float smoothTime;

    private void Start()
    {
        offset = transform.position - player.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothTime * Time.smoothDeltaTime);
    }
}
