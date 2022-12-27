using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
    public AudioSource rollingSound;
    public float ballSpeed;
    public float maxSpeed = 500;
    public float extraSpeed = 1;
    public int ballsLeft = 3;

    bool isGrounded;
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

        if (!rollingSound.isPlaying && isGrounded)
        {
            rollingSound.Play();
        }
        else if(!isGrounded || (rb.velocity.magnitude<5 && rollingSound.isPlaying))
        {
            rollingSound.Stop();
        }
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
            other.enabled = false;
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
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "DestructionLayer")
        {
            LostChance();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Bridge")
        {
            PlayObjectAnimation(collision.transform);
        }
        if (collision.transform.tag == "Obstacle")
        {
            AudioManager.instance.PlaySFX("Obstacle"+Random.Range(1,3).ToString());
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!isGrounded && rb.velocity.magnitude > 5)
            if (collision.transform.tag == "Ground")
                isGrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (isGrounded)
            if (collision.transform.tag == "Ground")
                isGrounded = false;
    }
    #endregion

    #region Object Collide Functions
    void PlayObjectAnimation(Transform obj)
    {
        obj.GetComponent<Animator>().enabled = true;
    }
    public void GetCoin(GameObject obj)
    {
        AudioManager.instance.PlaySFX("Coin");
        Destroy(obj);
        GameManager.instance.UpdateCoins(1);
    }
    public void GetKey(GameObject obj)
    {
        AudioManager.instance.PlaySFX("Key"+Random.Range(1,3).ToString());
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
        AudioManager.instance.PlaySFX("Fail");
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
        AudioManager.instance.PlaySFX("Win");
        VfxHandler.instance.PlayParticle(nameof(LevelEnd), Vector3.zero);
        Invoke(nameof(InvokeNewLevel),3);
        rb.isKinematic = true;
    }
    void InvokeNewLevel()
    {
        rb.isKinematic = false;
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

        if(xSpeed >200)
            xSpeed = Mathf.Clamp(xSpeed, -maxSpeed, maxSpeed);
            ySpeed = Mathf.Clamp(ySpeed, -maxSpeed, maxSpeed);
       
        if(ySpeed > 0 || ySpeed < 0)
            rb.AddForce(new Vector3( cam.forward.x,0,cam.forward.z)* ySpeed *extraSpeed* ballSpeed * Time.deltaTime);

        if(xSpeed > 500 || xSpeed < 500)
            rb.AddForce(new Vector3( cam.right.x,0,cam.right.z) * xSpeed *extraSpeed* ballSpeed * Time.deltaTime);
      
    }
    public float distanceFromObject;
    void camMove()
    {
        Vector3 lookOnObject = transform.position - cam.position;
        cam.forward = lookOnObject.normalized;
        Vector3 playerLastPosition;
        playerLastPosition = transform.position - lookOnObject.normalized * distanceFromObject;
        playerLastPosition.y = transform.position.y + distanceFromObject/1.5f;
        cam.position = Vector3.Lerp(cam.position, playerLastPosition, Time.deltaTime * 20); 
    }
    #endregion
}
