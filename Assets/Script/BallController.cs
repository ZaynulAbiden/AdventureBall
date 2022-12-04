using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        rb = GetComponent<Rigidbody>();

    }

    #endregion
    #region Data Member
    Rigidbody rb;
    public float speed;
    public float maxSpeed=50;
    Vector3 mousePos;
    public int ballsLeft = 3;
    #endregion

    public void SetBallPosition(Vector3 position)
    {
        transform.position = position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    #region Unity Methods


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            mousePos = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            Movement();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LevelEnd")
        {
            LevelEnd();
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
    #endregion

    void LostChance()
    {
        SetBallPosition(Levels.instance.currentLevel.Find("StartingPoint").transform.position);
        if (--ballsLeft == 0)
        {
            GameManager.instance. GameOver();
            return;
        }
        GameManager.instance.UpdateBallsLeftContainer(ballsLeft);
    }
    void LevelEnd()
    {
        GameManager.instance.NextLevel();
    }

    #region Movement Function
    void Movement()
    {
        if (!GameManager.instance.isGameRunning)
            return;

        if (mousePos.y + 1f < Input.mousePosition.y)
            rb.AddForce(Vector3.forward * speed*100* Time.deltaTime,ForceMode.Impulse);
        else if (mousePos.y - 2f > Input.mousePosition.y)
            rb.AddForce(-Vector3.forward * speed * 100 * Time.deltaTime, ForceMode.Impulse);

        if (mousePos.x + 50 < Input.mousePosition.x)
            rb.AddForce(Vector3.right * speed * 200 * Time.deltaTime, ForceMode.Impulse);
        else if (mousePos.x - 50 > Input.mousePosition.x)
            rb.AddForce(-Vector3.right * speed *200* Time.deltaTime, ForceMode.Impulse);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    #endregion
}
