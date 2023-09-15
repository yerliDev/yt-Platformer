using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject player;
    [SerializeField] private Image imgFinger;
    [SerializeField] private GameObject tapToStart;
    [SerializeField] private GameObject preGameUI;
    [FormerlySerializedAs("levelButton")] [SerializeField] private GameObject levelPassButton;
    [SerializeField] private GameObject levelFailButton;
    public bool finishedGame = false;
    private static readonly int StartedGame = Animator.StringToHash("startedGame");

    private void Start()
    {
        
        Application.targetFrameRate = 60;

        anim.SetBool(StartedGame,false);
        tapToStart.transform.DOScale(1.1f, 0.5f).SetLoops(10000, LoopType.Yoyo).SetEase(Ease.InOutFlash);
        imgFinger.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-200f,-166f), 1f)
            .SetLoops(10000, LoopType.Yoyo).SetEase(Ease.InOutFlash);
        player = GameObject.FindWithTag("Player");
        levelPassButton.SetActive(false);
        levelFailButton.SetActive(false);
        finishedGame = false;
        
        player.GetComponent<PlayerManager>().enabled = false;
        

    }

    private void ActivatePlayer()
    {
        preGameUI.SetActive(false);
        player.GetComponent<PlayerManager>().enabled = true;
        anim.SetBool(StartedGame,true);
    }

    // Start is called before the first frame update
    

    public void ActivateEndButton()
    {
        levelPassButton.SetActive(true);
    }
    public void ActivateFailButton()
    {
        levelFailButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0 && !finishedGame)
        {
            ActivatePlayer();
        }
       
    }
}
