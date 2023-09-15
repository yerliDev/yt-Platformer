using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.Serialization;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private List<Transform> pizzaBoxes = new List<Transform>();
    [FormerlySerializedAs("boxPlace")] [SerializeField] private Transform handPos; //Position of player's hand
    [SerializeField] private Transform pizzaPos; //Position of table
    [SerializeField] private GameObject ui;
    [SerializeField] private GameObject scoreManager;
    [SerializeField] private bool changeCam = true;
    public GameObject sfxManager;
    public GameObject cam;
    
    public float followSpeed = 50f;
    
    
    public Animator anim;
    
    private static readonly int DoesHavePizza = Animator.StringToHash("doesHavePizza");
    private static readonly int FinishedGame = Animator.StringToHash("finishedGame");
    private static readonly int GivingPizza = Animator.StringToHash("givingPizza");
    private static readonly int HitObstacle = Animator.StringToHash("hitObstacle");
    
    //Player x movement
    [Header("Reference")] 
    public Transform transToMove;

    public float speedModifier = .1f;
    public float xMin = -10f;
    public float xMax = 10f;
    public bool finishedLevel = false;
    private bool blockXMov = false;

    [Space] 
    public bool localMov;

    private Touch currentTouch;
    private Vector3 newPos = Vector3.zero;
    private static readonly int FailedLevel = Animator.StringToHash("failedLevel");

    public float speed = 5;
    public float smoothnessModifier = 0.2f;

    [SerializeField] float pizzaHeight = 0f;
    
    private void ChangeCameraTransform(Vector3 newOffset,Vector3 newRotation)
    {
        cam.GetComponent<SmoothCamera>().offset = newOffset;
        var temp = Quaternion.Euler(newRotation);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation,temp,Time.smoothDeltaTime*50f);
        changeCam = false;
    }

    private void BoxSound()
    {
        sfxManager.transform.GetChild(0).GetComponent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
        sfxManager.transform.GetChild(0).GetComponent<AudioSource>().Play();
    }

    private void WhooshSound()
    {
        sfxManager.transform.GetChild(1).GetComponent<AudioSource>().Play();
    }
    private void SlipSound()
    {
        sfxManager.transform.GetChild(4).GetComponent<AudioSource>().Play();
    }
    private bool SendPizzaToTable(Collider other)
    {
        if (pizzaBoxes.Count > 1)
        {
            pizzaPos = other.transform.GetChild(3).transform;
            pizzaBoxes.Last().DOMove(pizzaPos.position, 0.4f);
            pizzaBoxes.Last().parent = null;
            pizzaBoxes.Remove(pizzaBoxes.Last());
            WhooshSound();

            return true;
        }

        return false;
    }
    
    private bool SendPizzaToTableEnd(Transform pizzaPosition)
    {
        if (pizzaBoxes.Count > 1)
        {
            pizzaBoxes.Last().DOMove(pizzaPosition.position, 0.4f);
            pizzaBoxes.Last().parent = null;
            pizzaBoxes.Remove(pizzaBoxes.Last());
            WhooshSound();

            return true;
        }
        
        
        return false;
    }

    private IEnumerator SendPizzaAndEndLevel(Collider other,Transform pizzaP)
    {
        finishedLevel = true;
        float waitTime = 0.2f;
        
        while (SendPizzaToTableEnd(pizzaP))
        {
            pizzaP.Translate(0,0.15f,0);
            scoreManager.GetComponent<ScoreManager>().IncreasePoint();
            yield return new WaitForSeconds(waitTime);
            waitTime -= 0.01f;
            sfxManager.transform.GetChild(1).GetComponent<AudioSource>().pitch += 0.02f;
        }
        
        ui.GetComponent<UIManager>().finishedGame = true;
        anim.SetBool(FinishedGame,true);
        other.transform.GetChild(5).GetComponent<ParticleSystem>().Play(); //Emoji vfx
        other.transform.GetChild(0).GetComponent<ParticleSystem>().Play(); //Confetti vfx
        sfxManager.transform.GetChild(2).GetComponent<AudioSource>().Play(); //Confetti sfx
        //sfxManager.transform.GetChild(3).GetComponent<AudioSource>().Play(); //Cheer sfx
        ui.GetComponent<UIManager>().ActivateEndButton();
        
        yield break;
    }

    private IEnumerator Stumble()
    {
        anim.SetBool(HitObstacle,true);
        yield return new WaitForSeconds(0.3f);
        anim.SetBool(HitObstacle,false);
    }

    private void MovePlayerLeftRight()
    {
        if (Input.touchCount > 0 && !finishedLevel)
        {
            currentTouch = Input.GetTouch(0);

            if (currentTouch.phase == TouchPhase.Moved)
            {
                float newX = currentTouch.deltaPosition.x * speedModifier * Time.smoothDeltaTime;

                newPos = localMov ? transToMove.localPosition : transToMove.position;
                newPos.x += newX;
                newPos.x = Mathf.Clamp(newPos.x, xMin, xMax);
                
                newPos.y = transform.position.y;
                newPos.z = transform.position.z;
                

                if (localMov)
                {
                    transform.localPosition = newPos;
                }

                else
                    transform.position = newPos;
            }
        }
    }

    private void MovePlayerForward()
    {
        if(!finishedLevel)
            transform.position = Vector3.Lerp(transform.position,transform.position + Vector3.forward, Time.deltaTime * speed);
    }

    /*private void StackPizza()
    {
        for (int i = 1; i < pizzaBoxes.Count; i++)
        {
            var firstPizzaPosition = pizzaBoxes.ElementAt(i - 1).position;
            var secondPizzaPosition = pizzaBoxes.ElementAt(i).position;

            var secondPizza = pizzaBoxes.ElementAt(i).transform;

            secondPizzaPosition = new Vector3(firstPizzaPosition.x, firstPizzaPosition.y + 0.15f, handPos.position.z);
        }
    }*/
    
    /// <summary>
    /// UNITY EVENT FUNCTIONS
    /// </summary>
    private void Start()
    {
        ui.GetComponent<UIManager>().enabled = true;
        pizzaBoxes.Add(handPos);
        scoreManager = GameObject.FindWithTag("ScoreManager");
        scoreManager.GetComponent<ScoreManager>().scoreModifier = 10;
    }

    private void Update()
    {
        anim.SetBool(DoesHavePizza, pizzaBoxes.Count > 1);

        MovePlayerForward();
        
        if(!blockXMov)
            MovePlayerLeftRight();
        
        else
        {
            var position = transform.position;
            position = Vector3.MoveTowards(position,new Vector3(0,position.y,position.z),speed * Time.smoothDeltaTime); //Player will start to move automatically to center.
            transform.position = position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Pizza"))
        {
            Debug.Log("Collected a pizza!");
            var rotation = other.transform.rotation;
            rotation = Quaternion.Euler(rotation.x,Random.Range(-15f,15f),rotation.z); //Randomized angle of collected pizza
            other.transform.rotation = rotation;
            
            pizzaBoxes.Add(other.transform);
            other.gameObject.tag = "Untagged";
            other.transform.SetParent(handPos);
            other.transform.position = handPos.transform.position;
            other.transform.Translate(Vector3.up *  pizzaHeight);
            pizzaHeight += 0.15f;
            
            other.transform.GetChild(0).GetComponent<ParticleSystem>().Play(); //Smoke vfx
            BoxSound();
            
            
        }
        
        else if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Hit obstacle!");
            
            if (pizzaBoxes.Count > 1)
            {
                StartCoroutine(Stumble());
                cam.GetComponent<ScreenShake>().isShaking = true;
                pizzaBoxes.Last().DOJump(other.transform.position, 5f, 1, 3f); //Top pizza of the stack will jump towards air.
                pizzaBoxes.Last().parent = null;
                pizzaBoxes.RemoveAt(pizzaBoxes.Count-1); //Top pizza of the stack is removed.
                pizzaHeight -= 0.15f;
                
                WhooshSound();
                SlipSound();
            }

            else //Level fail
            {
                Debug.Log("Level failed!");
                anim.SetBool(FailedLevel,true);
                ui.GetComponent<UIManager>().finishedGame = true;
                finishedLevel = true;
                ui.GetComponent<UIManager>().ActivateFailButton();
                SlipSound();
            }
        }
        
        else if (other.CompareTag("LevelResult"))
        {
            Debug.Log("Calculating point");
            blockXMov = true;
            
            if (changeCam)
            {
                ChangeCameraTransform(new Vector3(20,15,-15),new Vector3(15,-50,0));
            }

            if (SendPizzaToTable(other))
            { 
                scoreManager.GetComponent<ScoreManager>().IncreasePoint();
                other.transform.GetChild(4).GetComponent<ParticleSystem>().Play();
            }
            
            else //Not collected all the pizza boxes available
            {
                Debug.Log("End of the level");
                finishedLevel = true;
                ui.GetComponent<UIManager>().finishedGame = true;
                anim.SetBool(FinishedGame,true);
                other.transform.GetChild(6).GetComponent<ParticleSystem>().Play();
                sfxManager.transform.GetChild(2).GetComponent<AudioSource>().Play();
                ui.GetComponent<UIManager>().ActivateEndButton();
                
            }
        }
        
        else if (other.CompareTag("Bonus"))
        {
            Debug.Log("BONUS TIME!!!");
            scoreManager.GetComponent<ScoreManager>().scoreModifier = 50;
        }
        
        else if (other.CompareTag("LevelEnd"))
        {
            Debug.Log("End of the level");
            ChangeCameraTransform(new Vector3(40,15,-30),new Vector3(15,-50,0));
            finishedLevel = true;
            ui.GetComponent<UIManager>().finishedGame = true;
            anim.SetBool(GivingPizza,true);
            var placeBox = other.transform.GetChild(3).transform;
            
            StartCoroutine(SendPizzaAndEndLevel(other,placeBox));
            
        }
    }
}

