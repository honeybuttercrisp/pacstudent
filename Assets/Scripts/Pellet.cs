using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int points = 10;

    protected virtual void Collect()
    {
        Object.FindFirstObjectByType<GameManager>().PelletCollected(this);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PacStudent"))
        {
            Collect();
        }
    }

}
