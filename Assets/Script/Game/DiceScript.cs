using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class DiceScript : MonoBehaviour
{
    [Header("Dice")]
    [SerializeField] Transform[] m_Direction = new Transform[6];
    [SerializeField] int[] m_Value = { 1, 2, 3, 4, 5, 6 };
    [Header("Attach")]
    [SerializeField] Rigidbody m_RB;
    [Header("Setting")]
    [SerializeField] Vector2 m_RandomPowY = new Vector2(4, 8);
    [SerializeField] Vector2 m_RandomRotX = new Vector2(50, 100);
    [SerializeField] Vector2 m_RandomRotY = new Vector2(50, 100);
    [SerializeField] int m_Delay = 100;

    bool m_isHit = false;

    async Task Roll()
    {
        m_RB.AddForce(new Vector3(0, Random.Range(m_RandomPowY.x, m_RandomPowY.y), 0), ForceMode.Impulse);
        m_RB.AddTorque(Vector3.right * Random.Range(m_RandomRotX.x, m_RandomRotX.y) + Vector3.up * Random.Range(m_RandomRotY.x, m_RandomRotY.y));

        while(m_RB.velocity.magnitude > 0.01f || !m_isHit)
        {
            await Task.Delay(m_Delay);
        }

        m_RB.velocity = Vector3.zero;
        m_RB.isKinematic = true;
    }

    public async Task<int> GetResult()
    {
        if (m_Direction.Length != m_Value.Length)
        {
            Debug.Log("<color=red>ダイスの設定が不適正です</color>");
            return 0;
        }

        await Roll();

        var dice = new Dictionary<float, int>(m_Direction.Length);
        for(int i = 0; i < m_Direction.Length; ++i)
            dice.Add(m_Direction[i].position.y, m_Value[i]);

        var result = dice.FirstOrDefault(x => x.Key.Equals(dice.Keys.Max()));

        return result.Value;
    }

    void OnCollisionStay(Collision collision)
    {
        m_isHit = true;
    }

    void OnCollisionExit(Collision collision)
    {
        m_isHit = false;
    }
}
