using UnityEngine;

/// <summary>
/// Liquidシェーダーを揺らすためのクラス
/// </summary>
/// 参考：https://www.youtube.com/watch?v=eIZgPAZx56s
/// https://www.youtube.com/watch?v=tI3USKIbnh0&list=LL&index=3&t=429s
/// ほぼ書き換えただけで原理はほぼ転用
public class WobbleMaterial : MonoBehaviour
{
    private Material m_material;

    private Vector3 m_prePosion;
    private Vector3 m_preRotation;

    [SerializeField] private float MaxWobble = 0.5f;
    [SerializeField] private float WobbleSpeed = 1.0f;
    [SerializeField] private float Recovery = 0.5f;

    private float m_wobbleAmountToAddX = 0.0f;
    private float m_wobbleAmountToAddZ = 0.0f;
    private float m_time = 0.0f;

    private const float TWO_PI = 2 * Mathf.PI;

    private readonly int m_PARAMATER_WOBBLEX = Shader.PropertyToID("_WobbleX");
    private readonly int m_PARAMATER_WOBBLEZ = Shader.PropertyToID("_WobbleZ");

    private void Start()
    {
        m_material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        var _delta = Time.deltaTime;
        m_time += _delta;

        // 揺れの減速を計算
        var _recovery = _delta * Recovery;
        m_wobbleAmountToAddX = Mathf.Lerp(m_wobbleAmountToAddX, 0, _recovery);
        m_wobbleAmountToAddZ = Mathf.Lerp(m_wobbleAmountToAddZ, 0, _recovery);

        // 実際に揺れの値として設定する値を計算
        var _pulse = TWO_PI * WobbleSpeed;
        var _sinPulse = Mathf.Sin(_pulse * m_time);
        var _wobbleAmountX = m_wobbleAmountToAddX * _sinPulse;
        var _wobbleAmountZ = m_wobbleAmountToAddZ * _sinPulse;

        // パラメータを参考元とはXとZを入れ替えて設定している
        // これによりX軸とZ軸の移動に合った揺れが表現される　元がなぜずれているのかは不明
        m_material.SetFloat(m_PARAMATER_WOBBLEZ, _wobbleAmountX);
        m_material.SetFloat(m_PARAMATER_WOBBLEX, _wobbleAmountZ);

        // 速度を計算する
        var _velocity = (m_prePosion - transform.position);
        var _angularVelocity = transform.rotation.eulerAngles - m_preRotation;
        
        // 速度と設定値から現在の揺れ速度を計算
        // この計算式だと反対側に移動を始めた時に不自然な揺れの減速が起きるので改善すべき
        m_wobbleAmountToAddX += Mathf.Clamp((_velocity.x + _angularVelocity.z) * MaxWobble, -MaxWobble, MaxWobble);
        m_wobbleAmountToAddZ += Mathf.Clamp((_velocity.z + _angularVelocity.x) * MaxWobble, -MaxWobble, MaxWobble);

        // 位置と回転を保存
        m_prePosion = transform.position;
        m_preRotation = transform.rotation.eulerAngles;
    }
}