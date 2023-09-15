using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class FpsCounter : MonoBehaviour
{
    [FormerlySerializedAs("FpsText")] [SerializeField] private TextMeshProUGUI fpsText;
    private const float PollingTime = 1f;
    private float time;
    private int frameCount;

    private void Update()
    {
        time += Time.deltaTime;
        frameCount++;
        if (time >= PollingTime)
        {
            var frameRate = Mathf.RoundToInt(frameCount / time);
            fpsText.text = frameRate.ToString() + " FPS";
            time -= PollingTime;
            frameCount = 0;
        }
    }
}
