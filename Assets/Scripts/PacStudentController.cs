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
    [SerializeField] private AudioSource wallThudSource;
    [SerializeField] private AudioClip wallThudSound; 
    [SerializeField] private GameObject wallCollisionEffectPrefab; 

    [SerializeField] private float moveDuration = 0.2f;
    private float gridSize = 1.0f;
    private Vector3 lastInput;
    private Vector3 currentInput;
    public Vector3 lastPosition;
    private bool isFirstMove = true;
    private bool wasMovingLastFrame = false; // Track if PacStudent was moving last frame

    void Awake()
    {
        tweener = GetComponent<Tweener>();

        audioSource.enabled = false;
        if (wallThudSource == null)
        {
            wallThudSource = gameObject.AddComponent<AudioSource>();
        }
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

        // If PacStudent couldn't move in either direction
        if (!moved)
        {
            if (wasMovingLastFrame && lastInput != Vector3.zero) // Only play thud if PacStudent was moving and trying to move
            {
                PlayWallThudSound();
                PlayWallCollisionEffect(lastInput);
            }
            StopMovement();
        }

        wasMovingLastFrame = moved;
    }

    private void PlayWallThudSound()
    {
        if (wallThudSound != null && wallThudSource != null)
        {
            wallThudSource.PlayOneShot(wallThudSound);
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

    public void StopMovement()
    {
        StopMovingSound();
        animator.enabled = false;
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

    public void StopMovingSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void PlayWallCollisionEffect(Vector3 direction)
    {
        if (wallCollisionEffectPrefab != null)
        {
            Vector3 collisionPoint = pacStudent.transform.position + (direction * (gridSize / 2));

            GameObject effect = Instantiate(wallCollisionEffectPrefab, collisionPoint, Quaternion.identity);

            float angle = 0;
            if (direction == Vector3.up) angle = 270;
            else if (direction == Vector3.down) angle = 90;
            else if (direction == Vector3.left) angle = 0;
            else if (direction == Vector3.right) angle = 180;

            effect.transform.rotation = Quaternion.Euler(0, 0, angle);

            Destroy(effect, 2f);
        }
    }
}