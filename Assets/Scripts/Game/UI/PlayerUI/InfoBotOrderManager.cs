using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// AIへの作戦UIを管理するクラス
/// </summary>
public class InfoBotOrderManager : MonoBehaviour
{
    [SerializeField] private ToggleGroup m_toggleGroup;
    [SerializeField] private InfoBotOrder m_prefab;

#if UNITY_EDITOR
    [Header("Debug"), SerializeField]
#endif
    private InfoBotOrder[] m_infoOrders;

    public void Initialize()
    {
        var _orders = ExtendMethod.GetSubClass<BotOrder_Template>();

        m_infoOrders = new InfoBotOrder[_orders.Length];
        for (int i = 0, len = _orders.Length; i < len; ++i)
        {
            m_infoOrders[i] = Instantiate(m_prefab, transform);
            m_infoOrders[i].Initialize(_orders[i].Name, i, m_toggleGroup);
        }
    }
}