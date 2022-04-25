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
    private readonly float edge = 0.5f;
    private void FixedUpdate()
    {
        if (moveStart)
        {
            Moving();
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

    /********************
     * セッターゲッター
     *******************/

    public void SetSpeedXY(float x, float y)
    {
        if (x < 0.0f) GoLeft();
        else GoRight();
        if (y < 0.0f) GoDown();
        else GoUp();

        speedX = Mathf.Abs(x);
        speedY = Mathf.Abs(y);
    }

    private float GetSpeedX()
    {
        return speedX * GetReverseX();
    }
    private float GetSpeedY()
    {
        return speedY * GetReverseY();
    }
    private float GetReverseX()
    {
        return plusX ? 1.0f : -1.0f;
    }
    private float GetReverseY()
    {
        return plusY ? 1.0f : -1.0f;
    }
    private void GoRight()
    {
        plusX = true;
    }

    private void GoLeft()
    {
        plusX = false;
    }

    private void GoUp()
    {
        plusY = true;
    }
    private void GoDown()
    {
        plusY = false;
    }
    /****************************/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            //Debug.Log("GoundTouch" + this.gameObject.name);
            Destroy(this.gameObject);
        }
        if (collision.transform.CompareTag("WallLeft"))
        {
            //Debug.Log("LeftTouch" + this.gameObject.name);
            GoRight();
        }
        if (collision.transform.CompareTag("WallRight"))
        {
            //Debug.Log("RightTouch" + this.gameObject.name);
            GoLeft();
        }
        if (collision.transform.CompareTag("WallUp"))
        {
            //Debug.Log("UpTouch" + this.gameObject.name);
            GoDown();
        }
        if (collision.transform.CompareTag("WallDown"))
        {
            //Debug.Log("DownTouch" + this.gameObject.name);
            GoUp();
        }
        if (collision.transform.CompareTag("Block"))
        {
            DicideBallAngle(collision.transform.position);
        }
    }

    private void DicideBallAngle(Vector3 block)
    {
        Vector3 result = this.transform.position - block;
        //Debug.Log(result);
        if (edge <= result.x)
        {
            Debug.Log("Right");
            GoRight();
        }
        else if (result.x <= -edge)
        {
            Debug.Log("Left");
            GoLeft();
        }
        else if (edge <= result.y)
        {
            Debug.Log("Up");
            GoUp();
        }
        else if (result.y <= -edge)
        {
            Debug.Log("Down");
            GoDown();
        }
    }
}
