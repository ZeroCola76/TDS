using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float backSpeed = 7f;
    private Rigidbody2D rb;
    private bool isJumping = false;
    private bool isBackingOff = false;
    public bool isGrounded = false;
    private bool backOffRequested = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        if (isBackingOff)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y); 
            return;
        }

        if (!isBackingOff)
        {
            Vector2 upwardOrigin = transform.position + new Vector3(-0.3f, 0.5f, 0);
            RaycastHit2D[] hitsAbove = Physics2D.RaycastAll(upwardOrigin, Vector2.up, 0.6f);
            Debug.DrawRay(upwardOrigin, Vector2.up * 0.6f, Color.yellow);

            if (!isBackingOff && !backOffRequested)
            {
                foreach (var hit in hitsAbove)
                {
                    if (hit.collider != null &&
                        hit.collider.gameObject != this.gameObject &&
                        hit.collider.gameObject.layer == gameObject.layer)
                    {
                        backOffRequested = true; 
                        StartCoroutine(BackOffRoutine());
                        return;
                    }
                }
            }
        }

        if (isJumping) return;


        Vector2 frontRayOrigin = transform.position + new Vector3(-0.5f, 0.4f, 0);
        Debug.DrawRay(frontRayOrigin, Vector2.left * 0.5f, Color.red);

        Vector2 backRayOrigin = transform.position + new Vector3(0, 0.4f, 0);
        Debug.DrawRay(backRayOrigin, Vector2.right * 0.5f, Color.blue);


        bool frontDetected = false;
        GameObject frontZombie = null;

        RaycastHit2D[] frontHits = Physics2D.RaycastAll(frontRayOrigin, Vector2.left, 0.5f);
        foreach (var hit in frontHits)
        {
            if (hit.collider != null &&
                hit.collider.gameObject != this.gameObject &&
                hit.collider.gameObject.layer == gameObject.layer)
            {
                frontDetected = true;
                frontZombie = hit.collider.gameObject;
                break;
            }
        }

        bool hasZombieBehind = false;
        RaycastHit2D[] backHits = Physics2D.RaycastAll(backRayOrigin, Vector2.right, 0.5f);
        foreach (var hit in backHits)
        {
            if (hit.collider != null &&
                hit.collider.gameObject != this.gameObject &&
                hit.collider.gameObject.layer == gameObject.layer)
            {
                hasZombieBehind = true;
                break;
            }
        }


        if (frontDetected && !hasZombieBehind && !isJumping)
        {
            ZombieMove frontMove = frontZombie.GetComponent<ZombieMove>();
            if (frontMove != null && frontMove.IsGrounded())
            {
                if (frontDetected && !hasZombieBehind && !isJumping && !AnyZombieIsJumping())
                {

                    JumpOver(frontZombie);
                }
            }
        }



        if (!frontDetected && isGrounded)
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
    }

    private bool AnyZombieIsJumping()
    {
        ZombieMove[] allZombies = FindObjectsOfType<ZombieMove>();

        foreach (var zombie in allZombies)
        {
            if (zombie != this &&
                zombie.gameObject.layer == gameObject.layer && 
                zombie.isJumping)
            {
                return true;
            }
        }

        return false;
    }


    private IEnumerator BackOffRoutine()
    {
        if (isBackingOff) yield break;

        isBackingOff = true;
        backOffRequested = false; 

        float backTime = 0.3f;
        float elapsed = 0f;

        while (elapsed < backTime)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        isBackingOff = false;
    }
    public bool IsBackingOff()
    {
        return isBackingOff;
    }

    private void JumpOver(GameObject frontObject)
    {
        if (isJumping) return;
        isJumping = true;

        Collider2D frontCollider = frontObject.GetComponent<Collider2D>();
        if (frontCollider == null) return;

        Vector3 topCenter = frontCollider.bounds.center + new Vector3(-0.1f, 0.1f, 0);

        float jumpHeight = 0.4f;
        float jumpDuration = 0.3f;

        transform.DOJump(topCenter, jumpHeight, 1, jumpDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => isJumping = false);
    }


    private void CheckGrounded()
    {
        Vector2 groundOrigin = transform.position + Vector3.down * 0.1f;
        int floorLayerMask = 0;

        switch (gameObject.layer)
        {
            case 3:
                floorLayerMask = LayerMask.GetMask("Floor1");
                break;
            case 9: 
                floorLayerMask = LayerMask.GetMask("Floor2");
                break;
            case 10:
                floorLayerMask = LayerMask.GetMask("Floor3");
                break;
            default:
                floorLayerMask = 0; 
                break;
        }

        RaycastHit2D hit = Physics2D.Raycast(groundOrigin, Vector2.down, 0.1f, floorLayerMask);
        isGrounded = hit.collider != null;

        Debug.DrawRay(groundOrigin, Vector2.down * 0.1f, isGrounded ? Color.green : Color.red);
    }


    public bool IsGrounded()
    {
        return isGrounded;
    }
}