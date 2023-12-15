using System.Collections.Generic;
using UnityEngine;

public class DamageUIManager : MonoBehaviour
{
    [SerializeField] private DamageUI m_prefab;

    private const int POOL_SIZE = 20;
    private List<DamageUI> m_pool;


    public void Initialize()
    {
        m_pool = new(POOL_SIZE);
        for(int i = 0; i < POOL_SIZE; ++i)
        {
            var _ui = Instantiate(m_prefab, transform);
            m_pool.Add(_ui);
        }
    }

    public void AddUI(AISystem ai_, float power_)
    {

        for(int i = 0; i < POOL_SIZE; ++i)
        {
            if(false == m_pool[i].IsUsed)
            {
                m_pool[i].Effect(ai_, power_);
                break;
            }
        }
    }
}