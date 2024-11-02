using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform tunnel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 pos = collision.transform.position;
        pos.x = tunnel.position.x;
        pos.y = tunnel.position.y;
        collision.transform.position = pos;
    }
}
