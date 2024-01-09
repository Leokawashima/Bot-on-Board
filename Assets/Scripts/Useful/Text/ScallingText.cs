using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// TMProの文字をサイズ
/// </summary>
/// AnimationTextクラスから実験をへて生まれた産物
public class ScallingText : CorutineMonoBehaivour
{
    [SerializeField] private TMP_Text m_text;

    [SerializeField] private float
        m_speedWave = 1.0f,
        m_speedFollow = 1.0f;

    [SerializeField] private float
        m_minScale = 0.0f,
        m_maxScale = 1.0f;
    protected override IEnumerator CoProcess()
    {
        while (true)
        {
            // 処理前にメッシュ更新
            m_text.ForceMeshUpdate();

            var _textInfo = m_text.textInfo;
            if (_textInfo.characterCount == 0)
            {
                yield break;
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

                var _sin = Mathf.Sin(Time.time * m_speedWave + i / m_speedFollow);

                var _scale = Vector3.one * Remap(_sin, 0.0f, 1.0f, m_minScale, m_maxScale);

                // 頂点参照しているindex取得
                int _vertexIndex = _charaInfo.vertexIndex;

                var _rotatedCenterVertex = (destVertices[_vertexIndex + 0] + destVertices[_vertexIndex + 2]) / 2;

                var _offset = _rotatedCenterVertex;

                // 一度オフセット(中心点座標)を抜いて行列計算
                destVertices[_vertexIndex + 0] -= _offset;
                destVertices[_vertexIndex + 1] -= _offset;
                destVertices[_vertexIndex + 2] -= _offset;
                destVertices[_vertexIndex + 3] -= _offset;

                // 行列変換
                Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, _scale);

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

            yield return null;
        }
    }

    private float Remap(float value, float from1, float from2, float to1, float to2)
    {
        return to1 + (value - from1) * (to2 - to1) / (from2 - from1);
    }
}