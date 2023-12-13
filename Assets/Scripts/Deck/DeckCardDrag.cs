using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// カードがドラッグされたときにイベントを送信するクラス
/// </summary>
public class DeckCardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// カードを保持するフィールド
    /// </summary>
    private MapObjectCard m_card;

    /// <summary>
    /// マウスの座標を補正するための値 左下からX+上にy+らしいためにずれる
    /// </summary>
    private static readonly Vector2 m_DISPLAY_SIZE = new Vector2(1920 / 2.0f, 1080 / 2.0f);

    public event Action<Vector2, MapObjectCard> OnBeginDrag;
    public event Action<Vector2> OnDrag, OnEndDrag;

    public void Initialize(MapObjectCard card_)
    {
        m_card = card_;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector2 GetMousePosition(PointerEventData eventData_)
    {
        return eventData_.position - m_DISPLAY_SIZE;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData_)
    {
        OnBeginDrag?.Invoke(GetMousePosition(eventData_), m_card);
    }

    void IDragHandler.OnDrag(PointerEventData eventData_)
    {
        OnDrag?.Invoke(GetMousePosition(eventData_));
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData_)
    {
        OnEndDrag?.Invoke(GetMousePosition(eventData_));
    }
}