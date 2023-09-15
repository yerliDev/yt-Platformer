
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private int index;

    private void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(index);
    }

    public void DelayedGo()
    {
        DOVirtual.DelayedCall(5, GoNextScene);
    }

    public void GoNextScene()
    { 
        SceneManager.LoadScene(index + 1); //Loads the next scene with index
    }
}
