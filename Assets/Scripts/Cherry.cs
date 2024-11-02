using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : Pellet
{
    protected override void Collect()
    {
        Object.FindFirstObjectByType<GameManager>().CherryCollected(this);
    }
}
