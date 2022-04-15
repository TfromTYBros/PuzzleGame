using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField] bool moveStart = false;
    [SerializeField] bool plusX = true;
    [SerializeField] bool plusY = true;
    [SerializeField] private float speedX = 0.0f;
    [SerializeField] private float speedY = 0.0f;

    private void FixedUpdate()
    {
        if (moveStart)
        {
            Moving();
        }
    }

    /********************
     * セッターゲッター
     *******************/

    public void SetSpeedXY(float x, float y)
    {
        speedX = x;
        speedY = y;
    }

    private float GetSpeedX()
    {
        return speedX * GetReverseX();
    }

    private float GetSpeedY()
    {
        return speedY * GetReverseY();
    }

    private void ChangePlusX()
    {
        if (plusX) plusX = false;
        else plusX = true;
    }

    private void ChangePlusY()
    {
        if (plusY) plusY = false;
        else plusY = true;
    }

    private float GetReverseX()
    {
        return plusX ? 1.0f : -1.0f;
    }

    private float GetReverseY()
    {
        return plusY ? 1.0f : -1.0f;
    }

    /****************************/
    /****************************/
 
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
            ChangePlusX();
        }
        if (collision.transform.CompareTag("Right"))
        {
            Debug.Log("RightTouch" + this.gameObject.name);
            ChangePlusX();
        }
        if (collision.transform.CompareTag("Up"))
        {
            Debug.Log("UpTouch" + this.gameObject.name);
            ChangePlusY();
        }
        if (collision.transform.CompareTag("Down"))
        {
            Debug.Log("DownTouch" + this.gameObject.name);
            ChangePlusY();
        }
    }

    public void Move()
    {
        moveStart = true;
    }

    private void Moving()
    {
        this.gameObject.transform.Translate(GetSpeedX(), GetSpeedY(), 0.0f);
    }

}
