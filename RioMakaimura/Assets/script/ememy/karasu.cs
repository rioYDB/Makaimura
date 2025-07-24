using UnityEngine;

public class karasu : MonoBehaviour
{
    public float e_moveSpeed = 3.0f;       // �G�̈ړ����x
    public float floatHeight = 0.5f;       // �㉺�̂ӂ�ӂ�̍���
    public float floatSpeed = 1.0f;        // �ӂ�ӂ�̑���

    float startY;                          // ����Y���W��ۑ�

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        // �ړ�������X���W���擾
        float newX = transform.position.x - e_moveSpeed * Time.deltaTime;

        // Y�͂ӂ�ӂ�̕������㉺�ɗh���
        float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        float newY = startY + offsetY;

        // �V�����ʒu�ɍX�V
        transform.position = new Vector3(newX, newY, transform.position.z);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}


