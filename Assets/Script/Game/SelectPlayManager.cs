using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 先攻後攻用
/// </summary>
/// 制作者　日本電子専門学校　ゲーム制作科　22CI0219　後藤
/// 改変者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class SelectPlayManager : MonoBehaviour
{
    public enum StateOrder { Non, First, Second }
    public StateOrder myOrder { get; private set; } = StateOrder.Non;

    [SerializeField] Button FirstAttackButton;
    [SerializeField] Button SecondAttack;
    [SerializeField] Button DecideButton;
    [SerializeField] Image SelectImage;
    [SerializeField] TextMeshProUGUI InductionText;

    public event Action SetFinishEvent;

    public void Initialize()
    {
        FirstAttackButton.onClick.AddListener(First);
        SecondAttack.onClick.AddListener(Second);
        DecideButton.onClick.AddListener(Decide);

        SelectImage.enabled = false;

        InductionText.text = "先攻 後攻を選んでください";
    }

    void First()
    {
        SelectImage.enabled = true;
        var pos = (FirstAttackButton.transform as RectTransform).anchoredPosition + new Vector2(200, 98);
        pos = new Vector2(pos.x * transform.lossyScale.x, pos.y * transform.lossyScale.y);
        SelectImage.transform.position = pos;
        myOrder = StateOrder.First;

        InductionText.text = "先攻でよろしいでしょうか";
    }

    void Second()
    {
        SelectImage.enabled = true;
        var pos = (SecondAttack.transform as RectTransform).anchoredPosition + new Vector2(200, 98);
        pos = new Vector2(pos.x * transform.lossyScale.x, pos.y * transform.lossyScale.y);
        SelectImage.transform.position = pos;
        myOrder = StateOrder.Second;

        InductionText.text = "後攻でよろしいでしょうか";
    }

    void Decide()
    {
        if (myOrder == StateOrder.Non) return;

        SetFinishEvent?.Invoke();
    }
}
