using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScrolllRoad : MonoBehaviour
{
    public GameObject backgroundParent; 
    public float moveSpeed = 5f;

    private float backgroundWidth;
    private GameObject backgroundClone;

    void Start()
    {
        if (backgroundParent == null)
        {
            enabled = false;
            return;
        }

        SpriteRenderer firstRenderer = backgroundParent.GetComponentInChildren<SpriteRenderer>();
        if (firstRenderer == null)
        {
            enabled = false;
            return;
        }
        backgroundWidth = firstRenderer.bounds.size.x;

        backgroundClone = Instantiate(backgroundParent, backgroundParent.transform.parent);
        backgroundClone.transform.position = backgroundParent.transform.position + Vector3.right * backgroundWidth;

        MoveBackground();
    }

    void MoveBackground()
    {
        float moveAmount = backgroundWidth * 2f;

        backgroundParent.transform.DOMoveX(backgroundParent.transform.position.x - moveAmount, moveAmount / moveSpeed)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .OnStepComplete(() =>
            {
                backgroundParent.transform.position += Vector3.right * moveAmount;
            });

        backgroundClone.transform.DOMoveX(backgroundClone.transform.position.x - moveAmount, moveAmount / moveSpeed)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .OnStepComplete(() =>
            {
                backgroundClone.transform.position += Vector3.right * moveAmount;
            });
    }
}
