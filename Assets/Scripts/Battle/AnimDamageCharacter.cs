using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class AnimDamageCharacter : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;

    private void OnEnable()
    {
        damageText.text = "";
    }

    private IEnumerator animDamage(int damage)
    {
        damageText.text = damage.ToString();
        damageText.rectTransform.DOShakePosition(0.5f, 0.1f);
        yield return new WaitForSeconds(0.5f);
        damageText.text = "";
    }
    public void TakeDamageAnim(int damage)
    {
        if (!damageText) return;
        StartCoroutine(animDamage(damage));
    }
}
