using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverLine : MonoBehaviour
{
    // ゲームオーバーかどうか（ゲームオーバー処理の無限発火を防ぐ）
    private bool isGameOver = false;
    // ボールがゲームオーバーラインに連続して触れている時間
    private float stayTime;

    // ゲームオーバーとなる時間のしきい値
    private const float STAY_THRESHOLD_TIME = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        stayTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // ボールがゲームオーバーラインに触れている時間を計算する
    // しきい値を超えたらゲームオーバーにする
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isGameOver && collision.CompareTag("Ball"))
        {
            stayTime += Time.deltaTime;
            if (stayTime > STAY_THRESHOLD_TIME)
            {
                isGameOver = true;
                GameManager.Instance.GameOver();
            }
        }
    }

    // ゲームオーバーラインからボールが離れたら触れている時間をリセットする
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            stayTime = 0.0f;
        }
    }
}
