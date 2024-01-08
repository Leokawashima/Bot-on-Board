using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleMenuTest : MonoBehaviour
{
    private readonly Vector2 Center = new(1920.0f / 2.0f, 1080.0f / 2.0f);

    private int index = 0;
    private Color color;

    private void OnEnable()
    {
        InputManager.Event_Position += OnPosition;
    }
    private void OnDisable()
    {
        InputManager.Event_Position -= OnPosition;
    }

    private void OnPosition()
    {
        var _vec = InputManager.Position - Center;
        var _degree = Mathf.Atan2(_vec.y, _vec.x) * Mathf.Rad2Deg;

        _degree = (_degree + 330.0f) % 360.0f;
        var _index = (int)(slice / (360.1f / _degree));

        if (_index != index)
        {
            m_images[index].color = color;
            index = _index;
            var col = m_images[index].color;
            color = col;
            m_images[index].color = Color.black;
        }
    }

    [SerializeField] Image m_prefab;
    [SerializeField, Range(2, 10)] int slice = 2;
    private List<Image> m_images;
    private void Start()
    {
        m_images = new(slice);
        for (int i = 0; i < slice; ++i)
        {
            var _img = Instantiate(m_prefab, transform);
            m_images.Add(_img);
            _img.color = Color.HSVToRGB((float)i / slice, 1, 1);
            _img.fillAmount = 1.0f / slice;
            _img.transform.rotation = Quaternion.Euler(0, 0, (float)i / slice * 360.0f + 240.0f);
        }
        color = m_images[index].color;
        m_images[index].color = Color.black;
    }
}