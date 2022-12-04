using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Throwing : MonoBehaviour
{

    Vector2 mousePos;                        //マウスpositionのマーカー位置を入れる変数（UIは2つの数字で表せる）
    Ray ray;                                  　　 //Ray型の変数rayを用意します
    RaycastHit hit;                     　　  //RaycastHit型の変数hitを用意します（レイが当たったオブジェクトの情報を得ます）
    public GameObject player;　　 // playerオブジェクトを格納する変数playerを用意します
    private Image cursorImg;             //クリック時に現れるcursorの絵マーカーを入れる変数
    private Vector3 forceDir;            //  力を加える方向を格納する変数forceDirを用意します
    Vector3 clickHitPos;

    public GameObject throwingObj;　　　//発射する弾のオブジェクトを入れる変数を用意します
    private Vector3 targetPos; 　　　　　//クリックした位置(弾の到達地点)を入れる変数を用意します
    public float throwingAngle;　　　　　//打ち上げる角度を入れる変数を用意します



private void Start()
    {
        // "PopCursor" オブジェクトを探してそこにセットされてる＜Image＞を取り込みます
        cursorImg = GameObject.Find("PopCursor").GetComponent<Image>();
        cursorImg.enabled = false;		//　 cursorImgを非表示にします。
        clickHitPos = player.transform.position;　 　//　はじめにクリックした場所をPlayerの位置にしておきます

        Collider collider = GetComponent<Collider>();　	//　 colliderを入れます
        if (collider != null)　　　　　　　　　　　　　　　 //　 colliderがあれば・・
        {
            collider.isTrigger = true;　 　// 生成する弾と干渉しないようにするために「isTrigger」にしておきます
        }
    }
    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);  //変数rayにマウスのカーソルの場所を入れます

        if (Physics.Raycast(ray, out hit))　　　//　rayが何かに当たっていたら・・
        {
            Vector3 hitPos = hit.point;  　　　　//　rayが当たった場所の座標を格納する変数hitPosを用意する

            if (Input.GetMouseButtonDown(1))  //　もしマウスのクリックボタンが押されたら・・
            {
                Vector3 playerPos = player.transform.position;       // 3D空間のPlayerの位置を取り込みます
                clickHitPos = hitPos;  　　　　　　　　　　　  //そのクリックした時のhitPositionを格納します
                mousePos = Input.mousePosition;                //　マウスカーソルの場所を変数mousePosに入れます

                //// 物理計算で動かすメソッド
                forceDir = (playerPos - clickHitPos).normalized;        // Player の場所とクリックされた場所の差から、弾の向かう方向ベクトルを求めます
                targetPos = hitPos;                       //　 targetPosの座標にRayの座標を入れます

                ////mouseのクリックマークを表示させるメソッド
                cursorImg.GetComponent<RectTransform>().position = new Vector2(mousePos.x, mousePos.y);
                ClickVisible();　　　　 // クリックした場所にマーカーを出すメソッドに飛びます
                Invoke("ThrowingBurret", 0.3f);         // 弾を発射するメソッドへ行きます
            }
            Quaternion startRot = player.transform.rotation;  //　クオタニオン（4元数）型の変数startRotにプレイヤーの回転角度の値を格納します

            //プレイヤーの回転角度の値を、今の場所から弾を撃つ方向へ、滑らかに回していきます
            player.transform.rotation = Quaternion.Slerp(startRot, Quaternion.LookRotation(-forceDir), Time.deltaTime * 5.0f);


            if (Input.GetMouseButtonUp(1))  　　　　//　もしマウスのクリックボタンが離されたら・・
            {
                ClickVisible();　　　　　　　　　　　//　マーカーを表示するメソッドClickVisible()に飛びます
            }
        }
    }
    private void ThrowingBurret()
    {
        if (throwingObj != null && targetPos != null)　//投げるObjectと狙う場所の値がある場合は・・
        {
            // Burretオブジェクトを生成します
            GameObject bullet = Instantiate(throwingObj, this.transform.position, Quaternion.identity);

            float angle = throwingAngle;            // 入力された射出角度を変数angleに入れます
                                                    //ものに当たるように射出する強さ（射出速度）を計算するCaluculateVelocity（座標、目標座標、角度）で算出するスクリプトです
            Vector3 calVelocity = CalculateVelocity(this.transform.position, targetPos, angle);

            Rigidbody rigidBody = bullet.GetComponent<Rigidbody>();      　　　　　　　// 投げ出す弾のrigidbodyを変数に格納します
            rigidBody.AddForce(calVelocity * rigidBody.mass, ForceMode.Impulse);  　　　//計算された強さの力を、弾に加えます
            Destroy(bullet, 1.5f);         				　　　　 // 3秒後に作られた弾を壊します
        }
    }
    private Vector3 CalculateVelocity(Vector3 posA, Vector3 posB, float angle)
    {
        float rad = angle * Mathf.PI / 180;  // 射出角をラジアンに変換

        // 水平方向の距離xを2点間から求めます
        float x = Vector2.Distance(new Vector2(posA.x, posA.z), new Vector2(posB.x, posB.z));

        float y = posA.y - posB.y;　　　 // 垂直方向の距離yを2点間から求めます

        // 斜方投射を初速度について計算します
        float velo = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(velo))　　// 条件を満たすveloの値（初速）がなければ、（もしくは1つでなくて、たくさんあれば）・・
        {
            return Vector3.zero;　　//Vector3.zeroにします
        }
        else　　　　　　　　　　//それを満たす初速値があれば・・
        {
            return (new Vector3(posB.x - posA.x, x * Mathf.Tan(rad), posB.z - posA.z).normalized * velo);　　 // 方向ベクトルに速度veloをかけた値を返します
        }
    }
    public void ClickVisible()
    {
        cursorImg.enabled = true;	 //マーカーの絵を表示します
        Invoke("ClickInvisible", 4.0f);　　 //4秒後にClickInvisible()メソッドに飛びます
    }

    public void ClickInvisible()　　　　 //ClickInvisible()メソッドです
    {
        cursorImg.enabled = false;　　　 //マーカーの絵を非表示にします
    }
}

