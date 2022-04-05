using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    bool MoveStart = false;

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (MoveStart)
        {
            Moving();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            Debug.Log("GoundTouch" + this.gameObject.name);
            Destroy(this.gameObject);
        }
        if (collision.transform.CompareTag("Left"))
        {
            Debug.Log("LeftTouch" + this.gameObject.name);
        }
        if (collision.transform.CompareTag("Right"))
        {
            Debug.Log("RightTouch" + this.gameObject.name);
        }
        if (collision.transform.CompareTag("UpOrBlock"))
        {
            Debug.Log("UpOrBlockTouch" + this.gameObject.name);
        }
    }

    public void Move()
    {
        MoveStart = true;
    }

    private void Moving()
    {
        //this.gameObject.transform.Translate()
    }
}
