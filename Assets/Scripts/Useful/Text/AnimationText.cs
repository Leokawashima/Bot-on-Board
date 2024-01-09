using UnityEngine;
using TMPro;

/// <summary>
/// TMProの文字をアニメーションさせる
/// </summary>
/// 参考　https://gurutaka-log.com/unity-textmeshpro-rotate
/// いじくり倒して実験している
public class AnimationText : MonoBehaviour
{
    [SerializeField] private TMP_Text m_text;

    [SerializeField] private bool
        m_isMove = true,
        m_isRotate = true,
        m_isScale = true;

    [SerializeField] private float
        m_animationSpeedMove = 1.0f,
        m_animationSpeedRotate = 1.0f,
        m_animationSpeedScale = 1.0f;

    [SerializeField] private Vector2
        m_minMove = new(0.0f, 0.0f),
        m_maxMove = new(10.0f, 10.0f);

    [SerializeField, Range(0.0f, 360.0f)] private float
        m_minRotation = 0.0f,
        m_maxRotation = 180.0f;

    [SerializeField, Min(0.0f)] private float
        m_minScale = 0.0f,
        m_maxScale = 1.0f;

    private void Update()
    {
        // 処理前にメッシュ更新
        m_text.ForceMeshUpdate();

        var _textInfo = m_text.textInfo;
        if (_textInfo.characterCount == 0)
        {
            return;
        }

        // 文字毎にloop
        for (int i = 0, cnt = _textInfo.characterCount; i < cnt; ++i)
        {
            var _charaInfo = _textInfo.characterInfo[i];

            // ジオメトリない文字はスキップ
            if (false == _charaInfo.isVisible)
            {
                continue;
            }

            // Material参照しているindex取得
            int _materialIndex = _charaInfo.materialReferenceIndex;

            // 頂点(dest->destinationの略)
            Vector3[] destVertices = _textInfo.meshInfo[_materialIndex].vertices;

            var _sin = Mathf.Sin(Time.time + i);

            Vector3 _pos = m_isMove ?
                new Vector3(
                Remap(_sin * m_animationSpeedMove, 0.0f, 1.0f, m_minMove.x, m_maxMove.x),
                Remap(_sin * m_animationSpeedMove, 0.0f, 1.0f, m_minMove.y, m_maxMove.y),
                0.0f)
                :
                Vector3.zero;

            Quaternion _angle = m_isRotate ?
                Quaternion.Euler(0, 0, Remap(_sin * m_animationSpeedRotate, 0.0f, 1.0f, m_minRotation, m_maxRotation))
                :
                Quaternion.identity;

            Vector3 _scale = m_isScale ?
                Vector3.one * Remap(_sin * m_animationSpeedScale, 0.0f, 1.0f, m_minScale, m_maxScale)
                :
                Vector3.one;

            // 頂点参照しているindex取得
            int _vertexIndex = _charaInfo.vertexIndex;

            var _rotatedCenterVertex = (destVertices[_vertexIndex + 1] + destVertices[_vertexIndex + 2]) / 2;

            var _offset = _rotatedCenterVertex;

            // 一度オフセット(中心点座標)を抜いて行列計算
            destVertices[_vertexIndex + 0] -= _offset;
            destVertices[_vertexIndex + 1] -= _offset;
            destVertices[_vertexIndex + 2] -= _offset;
            destVertices[_vertexIndex + 3] -= _offset;

            // 行列変換
            Matrix4x4 matrix = Matrix4x4.TRS(_pos, _angle, _scale);

            destVertices[_vertexIndex + 0] = matrix.MultiplyPoint3x4(destVertices[_vertexIndex + 0]);
            destVertices[_vertexIndex + 1] = matrix.MultiplyPoint3x4(destVertices[_vertexIndex + 1]);
            destVertices[_vertexIndex + 2] = matrix.MultiplyPoint3x4(destVertices[_vertexIndex + 2]);
            destVertices[_vertexIndex + 3] = matrix.MultiplyPoint3x4(destVertices[_vertexIndex + 3]);

            // オフセット(中心点座標)を戻す
            destVertices[_vertexIndex + 0] += _offset;
            destVertices[_vertexIndex + 1] += _offset;
            destVertices[_vertexIndex + 2] += _offset;
            destVertices[_vertexIndex + 3] += _offset;
        }

        // ジオメトリ更新
        for (int i = 0, len = _textInfo.meshInfo.Length; i < len; i++)
        {
            // メッシュ情報
            _textInfo.meshInfo[i].mesh.vertices = _textInfo.meshInfo[i].vertices;
            m_text.UpdateGeometry(_textInfo.meshInfo[i].mesh, i);
        }
    }

    private float Remap(float value, float from1, float from2, float to1, float to2)
    {
        return to1 + (value - from1) * (to2 - to1) / (from2 - from1);
    }
}