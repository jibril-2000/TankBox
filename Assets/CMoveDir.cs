using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMoveDir : MonoBehaviour
{

    private Image cursorImg;                                 //クリック時に現れるcursorの絵を入れる変数
    Vector2 mousePos;                                           //マウスpositionの位置を入れる変数（UIは2つの数字で表せる）
    public float visibleTime = 0.3f;                        // カーソル表示時間を入れます

    public GameObject player;　　	// playerオブジェクトを格納する変数playerを用意します
    private Vector3 forceDir;               //  力を加える方向を格納する変数forceDirを用意します

    public float pushPower = 0.01f;    	// 押す力を用意します、inspectorからも調整できるようpublic変数にします
    Rigidbody rigidBody;                    // rigidbody型の変数rigidBodyを用意します
    Ray ray;                                    	//レイ型の変数ray
    RaycastHit hit;                     	//RaycastHit型の変数hit　　レイが当たったオブジェクトの情報を格納します
    public Camera cam;      //カメラ型の変数camを用意します

 

void Start()
    {
        ////CursorPopのスクリプト（飛ばす方向を求める）

        //cursorImg = GameObject.Find("PopCursor").GetComponent<Image>();   // ”PopCursor（Canvas内）”を探してそこの”Image”を変数cursorImgに入れる
        //cursorImg.enabled = false;                                    // cursorを非表示にする
        player = GameObject.Find("Player");           // "Player"という名前のオブジェクトを探して入れます
    }
    void FixedUpdate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);          //　変数rayにカメラからマウスカーソルのいる位置にレイを飛ばします
        mousePos = Input.mousePosition;                                                         //　変数mousePosに現在のカーソル位置を入れます
        if (Physics.Raycast(ray, out hit))  			　　   //　もしレイが何かに当たったら・・
        {
            Vector3 hitPos = hit.point;  			　　　//　変数hitに入れられた値をVector3型の変数hitPosに入れます

            Debug.Log(hitPos);　　　　　　　　　　　　　	　　　　　　　　//　当たった場所の座標を表示する
            Debug.DrawRay(cam.transform.position, ray.direction * 50f, Color.red);   //DrawRay（Vector3スタート座標, 方向ベクトルとレイの長さ、色 ）

            ////CursorPopのスクリプト
            Vector3 playerPos = player.transform.position;       // 3D空間のPlayerの位置を取り込みます
                                                                 //クリック時に表示するカーソルポイントの絵の座標として、マウスのカーソル位置を入れます
           // cursorImg.GetComponent<RectTransform>().position = new Vector2(mousePos.x, mousePos.y);
            forceDir = (playerPos - hitPos).normalized;   //プレイヤーの位置からレイの当たった位置を引いて、ベクトルを求めます

            if (Input.GetMouseButton(0))  	　　//　もしMouseボタンが1回（MouseBottonDown）押されたら
            {
                // 方向ベクトル方向（x,0,z）に衝撃力を逆向きに加えます・・高さのｙ軸方向は０にします
                player.GetComponent<Rigidbody>().AddForce(new Vector3(-forceDir.x * pushPower, 0f, -forceDir.z * pushPower), ForceMode.Impulse);


            }
            if (Input.GetMouseButton(1))  	　　//　もしMouseボタンが1回（MouseBottonDown）押されたら
            {
                // 方向ベクトル方向（x,0,z）に衝撃力を逆向きに加えます・・高さのｙ軸方向は０にします
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;


            }
        }
    }
    public void ClickVisible()          //　ClickVisibleメソッド　　クリックした時に赤いカーソルを表示させるメソッドです　
    {
        cursorImg.enabled = true;       //　cursorImgをtrueにします（表示させます）
        Invoke("ClickInvisible", visibleTime);   //　visibleTime時間経った後に、”Clickinvisible”メソッドに飛びます
    }

    ////ClickInvisibleのスクリプト
    public void ClickInvisible()         //　ClickInvisibleメソッド　　クリックした時に赤いカーソルを消すメソッドです
    {
        cursorImg.enabled = false;      //　cursorImgをfalseにします（表示を消します）
    }
}





