using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DiceSystem : MonoBehaviour
{
    [Header("Attach")]
    [SerializeField] Rigidbody m_RB;
    [Header("DiceSetting")]
    [SerializeField] Transform[] m_Direction = new Transform[6];
    [SerializeField] int[] m_Value = { 1, 2, 3, 4, 5, 6 };
    [Header("RollSetting")]
    [SerializeField] Vector2 m_RandomPowY = new Vector2(6, 14);
    [SerializeField] Vector2 m_RandomRotX = new Vector2(50, 200);
    [SerializeField] Vector2 m_RandomRotY = new Vector2(50, 200);
    [SerializeField] int m_StopDelay = 100;
    [SerializeField] int m_StartDelay = 2000;

    bool m_IsHit = false;

    async Task Roll()
    {
        m_RB.isKinematic = false;

        Random.InitState(System.DateTime.Now.Millisecond);
        m_RB.AddForce(new Vector3(0, Random.Range(m_RandomPowY.x, m_RandomPowY.y), 0), ForceMode.Impulse);
        m_RB.AddTorque(Vector3.right * Random.Range(m_RandomRotX.x, m_RandomRotX.y) + Vector3.up * Random.Range(m_RandomRotY.x, m_RandomRotY.y));

        await Task.Delay(m_StartDelay);

        while(m_RB.velocity.magnitude > 0.0001f || !m_IsHit)
        {
            await Task.Delay(m_StopDelay);
        }

        m_RB.velocity = Vector3.zero;
        m_RB.isKinematic = true;
    }

    public async Task<int> GetResult()
    {
        if (m_Direction.Length != m_Value.Length)
        {
#if UNITY_EDITOR
            Debug.Log("<color=red>ダイスの設定が不適正です</color>");
#endif
            return 0;
        }

        await Roll();

        var _dice = new Dictionary<int, float>(m_Direction.Length);
        for(int i = 0; i < m_Direction.Length; ++i)
            _dice.Add(m_Value[i], m_Direction[i].position.y);

        var _result = _dice.FirstOrDefault(x => x.Value.Equals(_dice.Values.Max()));

        return _result.Key;
    }

#if UNITY_EDITOR
    [ContextMenu("GetResult")]
    async void TestRoll()
    {
        Debug.Log(UnityEditor.EditorApplication.isPlaying ? await GetResult() : "Editor Is Not PlayMode");
    }
#endif

    void OnCollisionStay(Collision collision)
    {
        m_IsHit = true;
    }
    void OnCollisionExit(Collision collision)
    {
        m_IsHit = false;
    }
}
