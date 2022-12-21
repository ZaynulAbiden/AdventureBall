using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BallController : MonoBehaviour
{
    #region Singleton
    public static BallController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

    }

    #endregion
    #region Data Member
    Vector3 mousePos;

    Transform startingPlace;
    public Transform checkPoint = null;
    [Header("Camera Settings")]
    public Transform cam;
    Vector3 camOffset = new Vector3(0, 5, -5);

    [Header("Ball Settings")]
    public Rigidbody rb;
    public float ballSpeed;
    public float maxSpeed = 500;
    public float extraSpeed = 1;
    public int ballsLeft = 3;
    #endregion
    #region Unity Methods

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
            mousePos = Input.mousePosition;
        if (Input.GetMouseButton(0))
            Movement();
        if (GameManager.instance.isGameRunning)
            camMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CheckPoint")
        {
            checkPoint = other.transform;
        }
        if (other.tag == "EndPoint")
        {
            LevelEnd();
            return;
        }
        if (other.tag == "Coin")
        {
            GetCoin(other.gameObject);
            return;
        }
        if (other.tag == "Key")
        {
            GetKey(other.gameObject);
            return;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Bridge")
        {
            PlayObjectAnimation(collision.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "DestructionLayer")
        {
            LostChance();
        }
    }
    #endregion

    #region Object Collide Functions
    void PlayObjectAnimation(Transform obj)
    {
        obj.GetComponent<Animator>().enabled = true;
        print("done");

    }
    public void GetCoin(GameObject obj)
    {
        Destroy(obj);
        GameManager.instance.UpdateCoins(1);
    }
    public void GetKey(GameObject obj)
    {
        Destroy(obj);
        GameManager.instance.UpdateKeys(1);
    }
    #endregion

    public void SetBallPosition()
    {
        if (checkPoint!=null)
        {
            transform.position = checkPoint.position;
            transform.eulerAngles = checkPoint.eulerAngles;
        }
            
        else
        {
            startingPlace = Levels.instance.currentLevel.transform.Find("StartingPoint").transform;
            transform.position = startingPlace.position;
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        cam.position = transform.position + camOffset;
        camMove();
    }
    void LostChance()
    {
        SetBallPosition();
        if (--ballsLeft == 0)
        {
            GameManager.instance.GameOver();
            return;
        }
        GameManager.instance.UpdateBallsLeftContainer(ballsLeft);
    }
    void LevelEnd()
    {
        checkPoint = null;
        Levels.instance.LevelSetup(++Levels.instance.selectedLevelIndex);
        GameManager.instance.levelNoTxt.text = "Level " + (Levels.instance.selectedLevelIndex + 1);
    }

    #region Movement Function
    void Movement()
    {
        if (!GameManager.instance.isGameRunning)
            return;

        float ySpeed = Input.mousePosition.y - mousePos.y;
        float xSpeed = Input.mousePosition.x - mousePos.x;
        xSpeed = Mathf.Clamp(xSpeed, -maxSpeed, maxSpeed);
        ySpeed = Mathf.Clamp(ySpeed, -maxSpeed, maxSpeed);


        if(ySpeed > 0 || ySpeed < 0)
            rb.AddForce(cam.forward* ySpeed *extraSpeed* ballSpeed * Time.deltaTime);

        if(xSpeed > 100 || xSpeed < 100)
            rb.AddForce(cam.right * xSpeed *extraSpeed* ballSpeed * Time.deltaTime);

    }
    public float distanceFromObject;
    void camMove()
    {
        Vector3 lookOnObject = transform.position - cam.position;
        cam.forward = lookOnObject.normalized;
        Vector3 playerLastPosition;
        playerLastPosition = transform.position - lookOnObject.normalized * distanceFromObject;
        playerLastPosition.y = transform.position.y + distanceFromObject/2;
        cam.position = Vector3.Lerp(cam.position, playerLastPosition, Time.deltaTime * 10); 
    }
   
    #endregion
}
