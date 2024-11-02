using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 10.0f;

    protected override void Collect()
    {
        Object.FindFirstObjectByType<GameManager>().PowerPelletCollected(this);
    }
}
