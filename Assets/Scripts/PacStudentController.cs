using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    [SerializeField] private GameObject pacStudent;
    private Tweener tweener;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float moveDuration = 0.2f;
    private float gridSize = 1.0f;
    private Vector3 lastInput;
    private Vector3 currentInput;
    private Vector3 lastPosition;
    private bool isFirstMove = true;

    void Awake()
    {
        tweener = GetComponent<Tweener>();
        // Ensure audio is disabled at start
        audioSource.enabled = false;
        // Ensure animation is stopped at start
        animator.enabled = false;
    }

    void Update()
    {
        // Get player input
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isFirstMove) EnableAudio();
            lastInput = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (isFirstMove) EnableAudio();
            lastInput = Vector3.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (isFirstMove) EnableAudio();
            lastInput = Vector3.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (isFirstMove) EnableAudio();
            lastInput = Vector3.right;
        }

        // Move only if not currently tweening
        if (tweener.IsTweenComplete())
        {
            TryMove();
        }
    }

    private void EnableAudio()
    {
        audioSource.enabled = true;
        isFirstMove = false;
    }

    private void TryMove()
    {
        bool moved = false;
        lastPosition = pacStudent.transform.position;

        // Calculate target position
        Vector3 targetPosition = pacStudent.transform.position + lastInput * gridSize;

        if (IsWalkable(targetPosition))
        {
            // Update direction and move
            currentInput = lastInput;
            StartMovement(targetPosition, lastInput);
            moved = true;
        }
        else if (currentInput != Vector3.zero)
        {
            // Attempt to move in currentInput direction if lastInput is blocked
            targetPosition = pacStudent.transform.position + currentInput * gridSize;

            if (IsWalkable(targetPosition))
            {
                StartMovement(targetPosition, currentInput);
                moved = true;
            }
        }

        // If we couldn't move in either direction
        if (!moved)
        {
            StopMovement();
        }
    }

    private bool IsWalkable(Vector3 targetPosition)
    {
        float boxSize = gridSize * 0.8f;
        Collider2D hitCollider = Physics2D.OverlapBox(targetPosition, new Vector2(boxSize, boxSize), 0, LayerMask.GetMask("Obstacle"));
        return hitCollider == null;
    }

    private void StartMovement(Vector3 targetPosition, Vector3 direction)
    {
        tweener.AddTween(pacStudent.transform, pacStudent.transform.position, targetPosition, moveDuration);

        // Enable animator and play walk animation
        animator.enabled = true;
        if (direction == Vector3.up) animator.Play("Walk_Up");
        else if (direction == Vector3.down) animator.Play("Walk_Down");
        else if (direction == Vector3.left) animator.Play("Walk_Left");
        else if (direction == Vector3.right) animator.Play("Walk_Right");

        PlayMovingSound();
    }

    private void StopMovement()
    {
        StopMovingSound();
        animator.enabled = false;  // This will stop the animation
    }

    private void PlayMovingSound()
    {
        if (audioSource.enabled)
        {
            audioSource.loop = true;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
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