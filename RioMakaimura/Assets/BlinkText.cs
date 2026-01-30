using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    public float speed = 2f;
    TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float a = Mathf.Abs(Mathf.Sin(Time.unscaledTime * speed));
        text.alpha = a;
    }
}
