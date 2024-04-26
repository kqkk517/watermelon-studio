using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // ボールの種類を識別するID
    public int id { get; set; }
    // 落ちたかどうか
    public bool hasDropped { get; set; } = false;
    // 合体できるかどうか
    public bool canMerge { get; set; } = true;

    private Rigidbody2D rb;

    // カーソル関連の定数
    public const float MAX_X_POS = 2.8f;
    private const float Y_POS = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // すでに落ちていたら何もしない
        if (hasDropped)
        {
            return;
        }
        // クリックしたら落とす
        if (Input.GetMouseButton(0))
        {
            Drop();
        }
        // カーソルに合わせてボールを移動する
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.x = Mathf.Clamp(mousePos.x, -MAX_X_POS, MAX_X_POS);
        mousePos.y = Y_POS;
        transform.position = mousePos;
    }

    // 同じ種類のボールとぶつかったら合体する（ぶつかったボール同士を消す）
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball colBall = collision.gameObject.GetComponent<Ball>();
        if (colBall != null &&
            id == colBall.id &&
            id < GameManager.Instance.ballsLength &&
            canMerge &&
            colBall.canMerge)
        {
            canMerge = false;
            colBall.canMerge = false;
            // ぶつかった2つのボールの座標の中点にひとつ大きいボールを生成する
            GameManager.Instance.MergeBalls(
                (transform.position + colBall.transform.position) / 2, id);
            Destroy(gameObject);
            Destroy(colBall.gameObject);
        }
    }

    // ボールを落とす
    private void Drop()
    {
        hasDropped = true;
        // 物理演算をONにする
        rb.simulated = true;
        // 次のボールを生成可能にする
        GameManager.Instance.isNext = true;
    }
}
