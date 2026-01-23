using UnityEngine;

public class Boss_door : MonoBehaviour
{
    public Collider2D doorCollider;   // 通行判定
    public Animator animator;         // 扉アニメ（任意）

    private bool isOpen = false;

    void Start()
    {
        CloseDoor();
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;

        // 当たり判定を消す
        if (doorCollider != null)
            doorCollider.enabled = false;

        // アニメ再生
        if (animator != null)
            animator.SetTrigger("Open");

        Debug.Log("ボス扉が開いた！");
    }

    void CloseDoor()
    {
        isOpen = false;

        if (doorCollider != null)
            doorCollider.enabled = true;

        if (animator != null)
            animator.SetTrigger("Close");
    }
}
