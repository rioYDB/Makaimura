using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    public float speed = 1f;
    TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float a = Mathf.Abs(Mathf.Sin(Time.time * speed));
        text.alpha = a;
    }
}
