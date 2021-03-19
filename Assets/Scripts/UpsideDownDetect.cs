using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpsideDownDetect : MonoBehaviour
{
    public PlayerMove player;

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Bounce"))
        {
            player.reset();
        }
    }
}
