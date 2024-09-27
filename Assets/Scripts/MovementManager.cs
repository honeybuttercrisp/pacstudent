using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [SerializeField] private GameObject pacStudent; 
    private Tweener tweener;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    private Vector3[] node;
    private int currentNodeIndex;

    [SerializeField] float moveDuration;

    void Start()
    {
        moveDuration = 1.0f;

        node = new Vector3[]
        {
            pacStudent.transform.position,  
            pacStudent.transform.position + new Vector3(5, 0, 0),
            pacStudent.transform.position + new Vector3(5, -4, 0),   
            pacStudent.transform.position + new Vector3(0, -4, 0)        
        };

        tweener = GetComponent<Tweener>();
        
        StopMovingSound();
    }

    void Update()
    {
        if (tweener.IsTweenComplete())
        {
            Move();
        }
    }
    private void Move()
    {
        Vector3 startPosition = pacStudent.transform.position;
        Vector3 targetPosition = node[currentNodeIndex];

        switch (currentNodeIndex)
        {
            case 1: // Move Right
                animator.Play("Walk_Right");
                PlayMovingSound();

                break;
            case 2: // Move Down
                animator.Play("Walk_Down");
                break;
            case 3: // Move Left
                animator.Play("Walk_Left");
                break;
            case 0: // Move Up
                animator.Play("Walk_Up");
                break;
        }

        tweener.AddTween(pacStudent.transform, startPosition, targetPosition, moveDuration);
        

        currentNodeIndex++;

        if (currentNodeIndex >= node.Length)
        {
            currentNodeIndex = 0;
        }
    }
    private void PlayMovingSound()
    {
        audioSource.loop = true;

        if (!audioSource.isPlaying)
        {
            audioSource.Play(); 
        }
    }

    private void StopMovingSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();  
        }
    }
}