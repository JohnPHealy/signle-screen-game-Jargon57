using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;
public class PlayerMove : MonoBehaviour
{
    public float MaxSpeed;
    public float AccelerationSpeed;
    float moveDir;

    public float MaxRotSpeed;
    public float RotAcc;
    float rotDir;

    Rigidbody2D  rb;

    public IsOnGround groundCheck2;
    public IsOnGround groundCheck1;

    public CinemachineVirtualCamera Camera;
    public AnimationCurve CameraFOV;

    public float boostForce;

    public Transform DeathFilter;

    public Vector3 StartPos;

    public float BounceForce;

    bool bounceWait = true;

    float BoostValue;

    playerScore score_;

    bool hasMoved;

    float flipDir;
    bool canFLip = true;
    bool hasFlipped;
    public float FlipForce;

    public GameObject Flames;
    public Vector3 offsetFlames;

    public GameObject flipEffect;

    float totalVelocityL;
    public Slider speedSlider;
    public float lerpAmmount = 0.1f;

    public Animator TextAnimator;

    bool isOnGround()
    {
        if (groundCheck1.isonground && groundCheck1.isonground)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

#if UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif

        StartPos = transform.position;

        score_ = GetComponent<playerScore>();
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        moveDir = ctx.ReadValue<float>();
    }

    public void Rotate(InputAction.CallbackContext ctx)
    {
        rotDir = ctx.ReadValue<float>();
    }

    public void Flip(InputAction.CallbackContext ctx)
    {
        flipDir = ctx.ReadValue<float>();
    }

    public void Reset()
    {
        reset();
    }

    private void FixedUpdate()
    {
        // Camera.m_Lens.OrthographicSize = CameraFOV.Evaluate(Mathf.Abs(rb.velocity.x));

        //basic movement
        if (moveDir != 0)
        {
           // Flames.GetComponent<ParticleSystem>().emissionRate = 200;

            if (rb.velocity.x > 0)
            {
                Flames.transform.position = transform.position + offsetFlames;
            }
            else
            {
                Flames.transform.position = transform.position - offsetFlames;
            }
        }
        else
        {
            //Flames.GetComponent<ParticleSystem>().emissionRate = 0;
        }

        if (!hasMoved && moveDir > 0)
        {
            hasMoved = true;
            score_.go();
        }

        if (isOnGround())
        {
            if (Mathf.Abs(rb.velocity.x) < MaxSpeed)
            {
                rb.AddForce(transform.right * moveDir * AccelerationSpeed);
            }
        }

        if (!isOnGround())
        {
            if (Mathf.Abs(rb.angularVelocity) < MaxRotSpeed)
            {
                rb.AddTorque(rotDir * RotAcc);
            }
        }

        //the thing on the bottom that will reset player
        Vector3 offset = new Vector3(transform.position.x, -20, transform.position.z);
        DeathFilter.position = offset;

        //boost applied in fixed update so it will not be affected by framerate
        switch (BoostValue)
        {
            case 0:
                break;

            case 1:
                if (rb.velocity.x > 1)
                {
                    rb.AddForce(transform.right * boostForce);
                }
                else if (rb.velocity.x < -1)
                {
                    rb.AddForce(transform.right * -boostForce);
                }
                break;

            case 2:
                if (rb.velocity.x > 1)
                {
                    rb.AddForce(Vector2.right * boostForce);
                }
                else if (rb.velocity.x < -1)
                {
                    rb.AddForce(Vector2.right * -boostForce);
                }
                break;
        }

        BoostValue = 0;

        if (flipDir > 0 && canFLip && !isOnGround())
        {
            canFLip = false;

            GameObject effectInstance = Instantiate(flipEffect, transform.position, Quaternion.Euler(Vector3.zero));
            Destroy(effectInstance, 2);

            TextAnimator.SetBool("IsFlip", true);

            if (rb.velocity.x > 0)
            {
                rb.AddTorque(FlipForce);
                rb.AddForce(Vector2.right * -FlipForce / 50, ForceMode2D.Impulse);         
            }
            else
            {
                rb.AddTorque(-FlipForce);
                rb.AddForce(Vector2.right * FlipForce / 50, ForceMode2D.Impulse);
            }
            hasFlipped = true;
            score_.modTime(-2);

            StartCoroutine(flipReset());
        }

        if (isOnGround())
        {
            canFLip = false;
            hasFlipped = false;
        }

        if (!isOnGround())
        {
            if (hasFlipped)
            {
                canFLip = false;
            }
            else
            {
                canFLip = true;
            }
        }

        //speed effect
        totalVelocityL = Mathf.Lerp(totalVelocityL, rb.velocity.magnitude, lerpAmmount);
        speedSlider.value = totalVelocityL;
    }



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Speed"))
        {
            BoostValue = 1;
        }
        
        if (collision.gameObject.CompareTag("Speed1"))
        {
            BoostValue = 2;
        }


        if (collision.gameObject.CompareTag("Death"))
        {
            reset();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bounce"))
        {
            if (bounceWait)
            {
                rb.AddForce(Vector2.up * BounceForce);
                StartCoroutine(resetBounce());
            }
        }
    }

    public void reset()
    {
        transform.position = StartPos;
        score_.Reset();

        hasMoved = false;

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    IEnumerator resetBounce()
    {
        bounceWait = false;
        yield return new WaitForSeconds(0.5f);
        bounceWait = true;
    }

    IEnumerator flipReset()
    {
        yield return new WaitForSeconds(1);
        TextAnimator.SetBool("IsFlip", false);

    }
}
