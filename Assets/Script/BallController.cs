using System.Collections;
using System.Collections.Generic;
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
        rb = GetComponent<Rigidbody>();

    }

    #endregion
    #region Data Member
    Vector3 mousePos;

    [Header("Camera Settings")]
    public Transform cam;
    public float camRotationSpeed;
    public bool camRotating;
    public Vector3 camPoint1;
    public Vector3 camPoint2;


    public int inverseControl = 1;
    [Header("Ball Settings")]
    Rigidbody rb;
    public float speed;
    public float maxSpeed=50;
    public int ballsLeft = 3;
    #endregion

    public void SetBallPosition(Vector3 position)
    {
        transform.position = position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        camPoint1 = transform.position + new Vector3(0, 5, -5);
        camPoint2 = transform.position + new Vector3(0, 5, 5);
    }

    #region Unity Methods


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            mousePos = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            Movement();
            return;
        }
        if (Input.GetMouseButtonUp(0))
        {
            camRotating= false;
        }
    }
    void LateUpdate()
    {
        if (!camRotating)
        {
            if (inverseControl == 1)
                cam.position = camPoint1;
            else
                cam.position = camPoint2;
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

        if (mousePos.y + 10f < Input.mousePosition.y)
        {
         //   rb.AddForce((Vector3.forward * inverseControl) * speed * 100 * Time.deltaTime, ForceMode.Impulse);
         //   rb.AddTorque((Vector3.forward * inverseControl) * speed * 5000 * Time.deltaTime);
          //  StartCoroutine(RotateCamUp());
        }
        else if (mousePos.y - 50f > Input.mousePosition.y)
        {
           // rb.AddForce((-Vector3.forward*inverseControl) * speed * 100 * Time.deltaTime, ForceMode.Impulse);
           // rb.AddTorque((-Vector3.forward * inverseControl) * speed * 5000 * Time.deltaTime);
           if(!camRotating)
            StartCoroutine(RotateCamDown());
        }
        if (mousePos.x + 100 < Input.mousePosition.x)
        {
         //   rb.AddForce((Vector3.right*inverseControl) * speed * 200 * Time.deltaTime, ForceMode.Impulse);
         //   rb.AddTorque((Vector3.right*inverseControl) * speed * 5000 * Time.deltaTime);

        }
        else if (mousePos.x - 100 > Input.mousePosition.x)
        {
         //   rb.AddForce((-Vector3.right*inverseControl) * speed * 200 * Time.deltaTime, ForceMode.Impulse);
          //  rb.AddTorque((-Vector3.right*inverseControl) * speed * 5000 * Time.deltaTime, ForceMode.Impulse);
        }

      //  rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }



    IEnumerator RotateCamDown()
    {
        camRotating = true;
        print("called");

        Vector3 positionTarget = Vector3.zero;
        float rotationTarget = 0;
        inverseControl *= -1;
        if (inverseControl == 1)
        {
            positionTarget = camPoint1;
            rotationTarget = 0;
        }
        else
        {
            positionTarget = camPoint2;
            rotationTarget = 180;
        }

        while (cam.position!= positionTarget)
        {
            print(cam.rotation.eulerAngles.y);
            cam.position = Vector3.Lerp(cam.position, positionTarget,  Time.deltaTime);
            cam.rotation = Quaternion.Lerp(cam.rotation, 
            Quaternion.Euler(cam.rotation.eulerAngles.x, rotationTarget, cam.rotation.eulerAngles.z), Time.deltaTime);
            yield return null;
        }
        camRotating = false;
    }

    #endregion
}
