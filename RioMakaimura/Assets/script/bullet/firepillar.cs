using UnityEngine;

public class Firepillar : bullet
{
    public float lifetime = 0.2f; // �܍U���̎������ԁi�Z���j

    protected override void BulletMoves(GameObject Enemy)
    {
        Debug.Log("���@���p�C�A�ŃA�^�b�N�I�I�I");
        Destroy(Enemy);
        Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime); // �����ō폜
    }


}
