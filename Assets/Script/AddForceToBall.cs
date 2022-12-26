using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceToBall : MonoBehaviour
{
    public float SpeedMultiplyer = 2;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform == BallController.instance.transform)
        {
            BallController.instance.rb.AddForce(BallController.instance.cam.up * SpeedMultiplyer * Time.deltaTime);
        }
    }
}
