using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieHP : MonoBehaviour
{
    public int HP = 10;
    public int maxHP = 10;

    private Image hpBarImage;

    private Animator animator;


    private void Start()
    {
        hpBarImage = FindHPBarImage(transform);
        animator = GetComponent<Animator>();
        UpdateHPBar();
    }

    private Image FindHPBarImage(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.name == "HPBar")
            {
                return child.GetComponent<Image>();
            }

            Image result = FindHPBarImage(child);
            if (result != null)
                return result;
        }

        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            HP -= 7;
            HP = Mathf.Clamp(HP, 0, maxHP);

            Destroy(collision.gameObject);

            UpdateHPBar();

            if (HP <= 0)
            {
                if (animator != null)
                {
                    animator.SetBool("IsDead", true);
                }

                Destroy(gameObject, 1f); 
            }
        }
    }

    private void UpdateHPBar()
    {
        if (hpBarImage != null)
        {
            hpBarImage.fillAmount = (float)HP / maxHP;
        }
    }
}
