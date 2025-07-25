using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public LayerMask groundLayer; // Inspectorで地面のLayerMaskを設定

    // 接地判定の結果を外部に公開するプロパティ
    public bool IsGrounded { get; private set; }

    void Update() // これが正しいUpdate()の定義
    {
        // ★修正：プレイヤー本体のCollider2DをBoxCollider2Dとして取得する
        // ※プレイヤーのColliderがCapsuleCollider2Dなら、CapsuleCollider2Dに修正してください
        //   player_control.csではBoxCollider2Dとして定義されているので、ここではBoxCollider2Dを使用します。
        BoxCollider2D playerBoxCollider = transform.parent.GetComponent<BoxCollider2D>();
        if (playerBoxCollider == null)
        {
            Debug.LogError("PlayerGroundCheck: プレイヤーにBoxCollider2Dが見つかりません！", this);
            return;
        }

        // BoxCollider2Dのサイズとオフセットを取得 (playerBoxColliderを使用)
        Vector2 colliderSize = playerBoxCollider.size;
        Vector2 colliderOffset = playerBoxCollider.offset;

        // Raycastの開始位置をColliderの底辺の中央から少し上に設定 (子オブジェクトのTransformを基準に調整)
        Vector2 rayOrigin = (Vector2)transform.position + colliderOffset + Vector2.down * (colliderSize.y / 2f - 0.05f); // 微調整
        float rayLength = 0.1f; // 短くして誤検知を減らす

        // デバッグ用にRayを表示
        Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);

        // 下方向にrayを飛ばして、指定したレイヤーのオブジェクトと接触しているかどうか判別する
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, groundLayer);

        IsGrounded = (hit.collider != null); // ヒットしていればtrue
    }

    // オプション：InspectorでRaycastの始点を視覚的に確認するためのギズモ
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.05f); // GroundCheckerの位置を示す
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.down * 0.1f); // Raycastの方向を示す
    }
}
