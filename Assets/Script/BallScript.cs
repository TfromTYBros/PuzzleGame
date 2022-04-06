using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    bool MoveStart = false;
    bool plus = true;

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
        if (collision.transform.CompareTag("Up") || collision.transform.CompareTag("Block"))
        {
            Debug.Log("UpOrBlockTouch" + this.gameObject.name);
            plus = false;
        }
    }

    public void Move()
    {
        MoveStart = true;
    }

    private void Moving()
    {
        this.gameObject.transform.Translate(0.0f, 0.1f * GetReverse(), 0.0f);
    }

    private float GetReverse()
    {
        return plus ? 1.0f : -1.0f;
    }
}
