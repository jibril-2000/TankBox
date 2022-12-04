using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CHeadDir : MonoBehaviour
{
    private Image cursorImg;                                 //クリック時に現れるcursorの絵を入れる変数
    Vector2 mousePos;                                           //マウスpositionの位置を入れる変数（UIは2つの数字で表せる）

    public GameObject player;　　// playerオブジェクトを格納する変数playerを用意します
    private Vector3 forceDir;            //  力を加える方向を格納する変数forceDirを用意します

    public float pushPower = 5.0f;    // 押す力を用意します、inspectorからも調整できるようpublic変数にします
    private Rigidbody rigidBody;       // rigidbody型の変数rigidBodyを用意します
    Ray ray;                                            //Ray型の変数ray
    RaycastHit hit;                                //RaycastHit型の変数hit　　Rayが当たったオブジェクトの情報
    public Camera cam;	                //Camera型の変数camを用意します
    Vector3 clickHitPos;　　　　　//マウスでクリックした位置を格納します
    public bool raylineOn;                 // rayを表示するグラグ（Raylineクラスから参照されます）
    void Start()
    {
        //cursorImg = GameObject.Find("PopCursor").GetComponent<Image>();　　//　PopCursorオブジェクトに入っているimageを入れます　
        //cursorImg.enabled = false;                                                                                        //　cursorを非表示にします
        clickHitPos = player.transform.position;		　//　まずクリックしたポジションとしてPlayer の位置を入れます
        clickHitPos.y = -1.0f;           //　y軸の高さを調整するために、この数値を入れてあります（各自適宜に）        　　
        rigidBody = player.GetComponent<Rigidbody>();   　 // playerオブジェクトのrigidbodyをrigidBody変数に入れます
        raylineOn = false;　　　　　　　　　　　　　　　//　raylineOnフラグをfalseにします
    }
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        mousePos = Input.mousePosition;             //　マウスカーソルのいる位置を変数mousePosに入れます
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPos = hit.point;  //　当たった場所の座標を格納する変数hitPosを用意する
            Debug.Log(hitPos);　　　//　当たった場所の座標を表示する
            //DrawRay（Vector3スタート座標, 方向ベクトルとレイの長さ、色 ）
            Debug.DrawRay(cam.transform.position, ray.direction * 50f, Color.red);

            if (Input.GetMouseButton(0))      //　もしMouseボタンが1回（MouseBottonDown）押されたら
            {
                raylineOn = true;　　　　　　　　//　raylineOnをtrueに変えます
                rigidBody.velocity = Vector3.zero;   //　クリックした時に速度を０にして動きを止めます
                Vector3 playerPos = player.transform.position;       // 3D空間のPlayerの位置を取り込みます
                //cursorImg.GetComponent<RectTransform>().position = new Vector2(mousePos.x, mousePos.y);
                clickHitPos = hitPos;    　　　　　　//　そのクリックした時のhitPositionを格納します
                clickHitPos.y = -1.0f;　　　　　　　 //　y軸の高さを調整するために、この数値を入れてあります（各自適宜に）
                //// 物理計算で動かすメソッド

                forceDir = (playerPos - clickHitPos).normalized;　　　 //クリックした方向へのベクトルを出して大きさ1にします 
                float angle = Mathf.Atan2(forceDir.x, forceDir.z) * Mathf.Rad2Deg;   //向かうベクトルを角度に変換します
                                                                                     //playerオブジェクトをクリックした方向へ向かせます              
                transform.eulerAngles = new Vector3(forceDir.x, angle + 180, forceDir.z);
            }


            if (Input.GetMouseButtonUp(0))  　//　もしMouseボタンが1回（MouseBottonDown）離されたら
            {
                raylineOn = false; //raylineOnフラグをfalseにしてRayの表示を止めます
                                   // 方向ベクトル方向に衝撃力を加えます
                rigidBody.AddForce(new Vector3(-forceDir.x * pushPower, 0f, -forceDir.z * pushPower), ForceMode.Impulse);
            }
        }
    }
}
