
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    
    public int gameScore = 0;
    public int scoreModifier = 10; //10x score gain --> At bonus part 50x score gain?

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void IncreasePoint()
    {
        gameScore += scoreModifier;
    }
}
