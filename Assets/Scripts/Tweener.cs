using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    private List<Tween> activeTweens = new List<Tween>();

    void Update()
    {
        for (int i = activeTweens.Count - 1; i >= 0; i--)
        {
            Tween tween = activeTweens[i];
            float timeFraction = (Time.time - tween.StartTime) / tween.Duration;


            if (timeFraction >= 1.0f)
            {
                tween.Target.position = tween.EndPos;
                activeTweens.RemoveAt(i); 
            }
            else
            {
                tween.Target.position = Vector3.Lerp(tween.StartPos, tween.EndPos, timeFraction);
            }
        }
    }
    public void AddTween(Transform target, Vector3 startPos, Vector3 endPos, float duration)
    {
        Tween tween = new Tween(target, startPos, endPos, Time.time, duration);
        activeTweens.Add(tween);
    }

    public bool IsTweenComplete()
    {
        return activeTweens.Count == 0;
    }
}
