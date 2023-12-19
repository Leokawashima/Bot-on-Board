using System.Collections.Generic;
using UnityEngine;
using AI;

public class FloatingUIManager : MonoBehaviour
{
    [SerializeField] private FloatingUI m_prefab;

    private const int POOL_SIZE = 20;
#if UNITY_EDITOR
    [SerializeField]
#endif
    private List<FloatingUI>
        m_poolRest,
        m_poolUsed;

    public void Initialize()
    {
        m_poolRest = new(POOL_SIZE);
        m_poolUsed = new(POOL_SIZE);
        for(int i = 0; i < POOL_SIZE; ++i)
        {
            var _ui = Instantiate(m_prefab, transform);
            m_poolRest.Add(_ui);
            _ui.Event_Finished += Restore;
            _ui.gameObject.SetActive(false);
        }
    }
    private void Restore(FloatingUI ui_)
    {
        m_poolUsed.Remove(ui_);
        m_poolRest.Add(ui_);
        ui_.gameObject.SetActive(false);
    }

    public void AddUI(AIAgent ai_, float power_, Color color_)
    {
        if (m_poolRest.Count != 0)
        {
            m_poolRest[0].gameObject.SetActive(true);
            m_poolRest[0].Effect(ai_, power_, color_);
            m_poolUsed.Add(m_poolRest[0]);
            m_poolRest.RemoveAt(0);
        }
    }
}