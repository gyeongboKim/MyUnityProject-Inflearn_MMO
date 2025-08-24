using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UI_ButtonEffectEx
{
    public static void AddUIClickEffect(this Button button, Sprite clickEffectSprite)
    {

        // 버튼의 원래 Sprite를 저장해둔다.
        Sprite originalSprite = button.image.sprite;

        // 마우스를 눌렀을 때 (PointerDown) 이미지를 바꾼다.
        button.gameObject.BindEvent((data) =>
        {   
            button.image.sprite = clickEffectSprite;  
        }, Define.UIEvent.PointerDown);

        // 마우스를 뗐을 때 (PointerUp) 원래 이미지로 되돌린다.
        button.gameObject.BindEvent((data) =>
        {
            button.image.sprite = originalSprite;
        }, Define.UIEvent.PointerUp);

        // 추가: 누른 상태로 버튼 밖으로 나가면 원래 이미지로 되돌린다.
        button.gameObject.BindEvent((data) =>
        {
            button.image.sprite = originalSprite;
        }, Define.UIEvent.Exit);

    }

    public static void AddUIHoverEffect(this Button button, float scaleMultiplier = 1.1f, float duration = 0.2f)
    {
        Vector3 originalScale = button.transform.localScale;
        Vector3 hoverScale = originalScale * scaleMultiplier;

        button.gameObject.BindEvent((data) =>
        {
            button.StartCoroutine(ScaleCoroutine(button.transform, hoverScale, duration));
        }, Define.UIEvent.Enter);

        button.gameObject.BindEvent((data) =>
        {
            button.StartCoroutine(ScaleCoroutine(button.transform, originalScale, duration));
        }, Define.UIEvent.Exit);

    }
    
    private static IEnumerator ScaleCoroutine(Transform target, Vector3 targetScale, float duration)
    {
        Vector3 startScale = target.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Ease out effect
            t = 1f - Mathf.Pow(1f - t, 3f);

            target.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        target.localScale = targetScale;
    }
}

