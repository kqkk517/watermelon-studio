using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // 他のクラスから使用するGameManagerインスタンス
    public static GameManager Instance { get; private set; }
    // 次のボールを生成可能かどうか
    // Ball.Drop()でtrueにする
    public bool isNext { get; set; }
    // ボールの種類数
    public int ballsLength { get; private set; }

    [SerializeField] private GameObject[] Balls;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private TextMeshProUGUI ResultScoreText;
    [SerializeField] private GameObject PlayerHand;

    private int score;

    // ボールの生成関連の定数
    private const float GEN_X_POS = 0.0f;
    private const float GEN_Y_POS = 3.5f;
    private const float GEN_INTERVAL = 2.0f;

    // プレイヤーハンド関連の定数
    private const float HAND_X_OFFSET = 2.3f;
    private const float HAND_Y_POS = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        isNext = false;
        ballsLength = Balls.Length;
        score = 0;
        SetScore();
        PlayerHand.SetActive(true);
        GenerateBall();
    }

    // Update is called once per frame
    void Update()
    {
        // 次のボールを生成可能になったら2秒後に生成する
        if (isNext)
        {
            isNext = false;
            Invoke("GenerateBall", GEN_INTERVAL);
        }
        // カーソルに合わせてプレイヤーハンドを移動する
        Vector2 handPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        handPos.x = Mathf.Clamp(handPos.x, -Ball.MAX_X_POS, Ball.MAX_X_POS) - HAND_X_OFFSET;
        handPos.y = HAND_Y_POS;
        PlayerHand.transform.position = handPos;
    }

    // スコアをUIテキストにセットする
    private void SetScore()
    {
        ScoreText.text = score.ToString();
    }

    // スコアを計算する
    private void CalcScore(int ballId)
    {
        score += (int)Mathf.Pow(2, ballId);
        SetScore();
    }

    // ボールを合体する（ひとつ大きいボールを生成する）
    public void MergeBalls(Vector3 genPos, int parentId)
    {
        // 一番大きいボールだったら何も生成しない
        if (parentId == ballsLength - 1)
        {
            return;
        }
        // 引数に与えられた座標にひとつ大きいボールを生成する
        Ball newBall = Instantiate(Balls[parentId + 1], genPos,
            Quaternion.identity).GetComponent<Ball>();
        newBall.id = parentId + 1;
        newBall.hasDropped = true;
        newBall.GetComponent<Rigidbody2D>().simulated = true;
        CalcScore(parentId);
    }

    // ボールを生成する
    private void GenerateBall()
    {
        // 生成するボールのインデックス
        int ballIdx = Random.Range(0, Balls.Length - 4);
        Ball ball = Instantiate(Balls[ballIdx],
            new Vector2(GEN_X_POS, GEN_Y_POS),
            Quaternion.identity).GetComponent<Ball>();
        ball.id = ballIdx;
    }

    // ゲームオーバー処理
    public void GameOver()
    {
        ResultScoreText.text = "SCORE: " + score.ToString();
        GameOverPanel.SetActive(true);
    }
}
