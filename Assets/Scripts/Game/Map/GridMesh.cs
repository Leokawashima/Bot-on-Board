using System.Collections.Generic;
using UnityEngine;
using Map.Stage;

public class GridMesh : MonoBehaviour
{
    [SerializeField] private MeshFilter m_meshFilter;
    [SerializeField] private MeshRenderer m_meshRenderer;

    private Mesh m_mesh;
    private Vector2Int m_size;

    public void Initialize(Vector2Int size_)
    {
        var _mesh = new Mesh();

        var _vertices = new List<Vector3>();
        for (int x = 0; x <= size_.x; ++x)
        {
            for (int z = 0; z <= size_.y; ++z)
            {
                _vertices.Add(new Vector3(x - size_.x / 2.0f, 0.0f, z - size_.y / 2.0f));
            }
        }

        var _triangles = new List<int>();
        for (int x = 0; x < size_.x; ++x)
        {
            for (int z = 0; z < size_.y; ++z)
            {
                SetTriangle(_triangles, x * size_.y + x + z, size_);
            }
        }

        _mesh.vertices = _vertices.ToArray();
        _mesh.subMeshCount = m_meshRenderer.materials.Length;
        _mesh.SetTriangles(_triangles, 0);
        _mesh.RecalculateNormals();
        m_meshFilter.sharedMesh = _mesh;
        m_mesh = _mesh;
        m_size = size_;
    }

    public void Refresh(MapStage stage_)
    {
        var _mesh = m_mesh;
        var _size = m_size;

        for (int i = 0, cnt = _mesh.subMeshCount; i < cnt; ++i)
        {
            var _triangles = new List<int>();
            for (int x = 0; x < _size.x; ++x)
            {
                for (int z = 0; z < _size.y; ++z)
                {
                    switch (i)
                    {
                        case 0:
                            if (stage_.Object[z][x] == null)
                                SetTriangle(_triangles, x * _size.y + x + z, _size);
                            break;
                        case 1:
                            if (stage_.Object[z][x] != null)
                                SetTriangle(_triangles, x * _size.y + x + z, _size);
                            break;
                        case 2:
                            if (stage_.Chip[z][x] == null)
                                SetTriangle(_triangles, x * _size.y + x + z, _size);
                            break;
                    }
                }
            }
            _mesh.SetTriangles(_triangles, i);
        }
    }

    private void SetTriangle(List<int> triangles_, int offset_, Vector2Int size_)
    {
        triangles_.AddRange(new int[6]
        {
            offset_, offset_ + 1, offset_ + size_.y + 1,
            offset_ + 1, offset_ + size_.y + 2, offset_ + size_.y + 1
        });
    }

    public class MeshBuilder
    {
        public List<CustomMesh> Meshs { get; private set; }

        public MeshBuilder()
        {
            Meshs = new();
        }
        public void Add(Vector3 pos_)
        {
            Meshs.Add(new CustomMesh(pos_));
        }
        public Mesh Build()
        {
            var _mesh = new Mesh();

            var _verts = new List<Vector3>();
            var _triangles = new List<int>();
            var _normals = new List<Vector3>();

            for (int i = 0; i < Meshs.Count; ++i)
            {
                _verts.AddRange(Meshs[i].Vertices);
                for (int j = 0; j < i; ++j)
                {
                    for (int k = 0; k < Meshs[i].Triangles.Count; ++k)
                    {
                        Meshs[i].Triangles[k] += Meshs[j].Vertices.Count;
                    }
                }
                _triangles.AddRange(Meshs[i].Triangles);
                _normals.AddRange(Meshs[i].Normals);
            }

            _mesh.vertices = _verts.ToArray();
            _mesh.SetTriangles(_triangles, 0);
            _mesh.RecalculateNormals();
            return _mesh;
        }

        public class CustomMesh
        {
            public List<Vector3> Vertices { get; private set; }
            public List<int> Triangles { get; private set; }
            public List<Vector3> Normals { get; private set; }

            public CustomMesh(Vector3 pos_)
            {
                Vertices = new List<Vector3>
                {
                    pos_,
                    pos_ + new Vector3(0.0f, 0.0f, 1.0f),
                    pos_ + new Vector3(1.0f, 0.0f, 1.0f),
                    pos_ + new Vector3(1.0f, 0.0f, 0.0f),
                };
                Triangles = new List<int>
                {
                    0, 1, 2,
                    0, 2, 3,
                };
                Normals = new List<Vector3>
                {
                    Vector3.up,
                    Vector3.up,
                    Vector3.up,
                    Vector3.up,
                };
            }
            public Mesh Build()
            {
                var _mesh = new Mesh
                {
                    vertices = Vertices.ToArray()
                };
                _mesh.SetTriangles(Triangles, 0);
                _mesh.RecalculateNormals();
                return _mesh;
            }
        }
    }
}