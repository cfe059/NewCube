using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private TextMeshProUGUI TMP;
    [SerializeField] private float textAppearDuration = 0.15f;
    [SerializeField] private float textDisappearDuration = 0.3f;
    [SerializeField] private float textJumpHeight = 30f;
    public int damage ;
    void Start()
    {
        // Sequence sequence = DOTween.Sequence();
        // sequence
        //     .Append(TMP.DOCounter(0, damage, 0.5f))
        //    // .Join(transform.DOMoveY(0.1f, 0.5f))
        //    // .Join(transform.DOShakeScale(1, 0.5f).SetEase(Ease.InBounce))
        //     .AppendInterval(1f)
        //     .OnComplete(()=> Destroy(gameObject)).Play();
        //
        TMP.text = damage.ToString();
        var tmpAnimator = new DOTweenTMPAnimator(TMP);

        for (var i = 0; i < tmpAnimator.textInfo.characterCount; i++)
        {
            tmpAnimator.DOScaleChar(i, 0.7f, 0);
            var charOffset = tmpAnimator.GetCharOffset(i);

            var sequence = DOTween.Sequence();

            // 登場
            sequence.Append(tmpAnimator.DOOffsetChar(i, charOffset + new Vector3(0f, textJumpHeight, 0f), textAppearDuration)
                    .SetEase(Ease.OutFlash, 2))
                .Join(tmpAnimator.DOFadeChar(i, 1f, textAppearDuration/2f))
                .Join(tmpAnimator.DOScaleChar(i, 1f, textAppearDuration)
                    .SetEase(Ease.OutBack))
                .SetDelay(0.05f * i);

            // タイミングを合わせて0.5秒待つ
            sequence.AppendInterval(0.05f * (tmpAnimator.textInfo.characterCount - i))
                .AppendInterval(0.5f);

            // 消滅
            sequence.Append(tmpAnimator.DOFadeChar(i, 0, textDisappearDuration));
            sequence.Join(tmpAnimator.DOOffsetChar(i, charOffset + new Vector3(0, textJumpHeight, 0), textDisappearDuration)
                .SetEase(Ease.Linear))
                .OnComplete(() => Destroy(gameObject))
                .Play();
        }
    }

    
}
