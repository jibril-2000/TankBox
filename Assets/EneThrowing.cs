using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EneThrowing : MonoBehaviour
{

    public GameObject player;　　 	// playerオブジェクトを格納する変数playerを用意します
    public GameObject cursorMaker;             //攻撃時に現れるcursorの絵マーカーを入れる変数
    private Vector3 forceDir;               //  力を加える方向を格納する変数forceDirを用意します

    public GameObject throwingObj;　　　//発射する弾のオブジェクトを入れる変数を用意します
    private Vector3 targetPos; 　		//相手の場所を入れる変数を用意します
    public float throwingAngle;   		  //打ち上げる角度を入れる変数を用意します
    public GameObject muzzle;  		 // 弾を発射する場所の空オブジェクトの場所を入れます
    public int rapid;                           		// 弾を発射する間隔の調整値
    private int i;
    private void Start()
    {
        // cursorMakerの位置にplayerオブジェクトの位置を入れます(yの位置だけ地面に近づけます)
        cursorMaker.transform.position = new Vector3(player.transform.position.x, -1.5f, player.transform.position.z);
        cursorMaker.SetActive(false);       //　 cursorImgを非表示にします。

        //clickHitPos = player.transform.position;   //　はじめにクリックした場所をPlayerの位置にしておきます
        targetPos = player.transform.position;        // playerの位置を変数targetPos入れます
        forceDir = Vector3.forward;                                // 基準位置を決めます
        int i = rapid;			　//　変数iに撃つ間隔の値を入れます	
    }
    void Update()
    {
        ////標的の位置に向かせる
        Quaternion startRot = player.transform.rotation;        //回頭を始めるはじめの角度をプレイヤーから取ります

        //Slerp関数でゆっくり、向かう方向まで回頭をさせます（始めの角度、向かう位置までの角度、時間）
        player.transform.rotation = Quaternion.Slerp(startRot, Quaternion.LookRotation(-forceDir), Time.deltaTime * 5.0f);

        targetPos = player.transform.position;               //Playerの位置を取り込みます

        // Player の場所とクリックされた場所の差から、弾の向かう方向ベクトルを求めます
        forceDir = (targetPos - gameObject.transform.position).normalized;

        i -= 1;         //　毎フレームごとに１づつ減らしていきます

    }
    void OnTriggerStay(Collider other)　　　　　//　もしトリガー内部に何かが存在し続けていたら
    {
        if (other.tag == "Player")　　　　　　　//　もしそれのTagが”Player”トならば・・
        {

            if (i <= 0)          //　そして変数　 i 　が0よりも小さければ・・        
            {
                i = rapid;			 //　i　に変数rapidの値をいれます
                ThrowingBurret();            // 弾を発射するメソッドへ行きます
            }
        }
    }
    private void ThrowingBurret()
    {
        if (throwingObj != null && targetPos != null) 	//投げるObjectと狙う場所の値がある場合は・・
        {

            // Burretオブジェクトを生成します
            GameObject bullet = Instantiate(throwingObj, muzzle.transform.position, Quaternion.identity);

            float angle = throwingAngle;            // 入力された射出角度を変数angleに入れます
                                                    //ものに当たるように射出する強さ（射出速度）を計算するCaluculateVelocity（座標、目標座標、角度）で算出するスクリプトです
            Vector3 calVelocity = CalculateVelocity(muzzle.transform.position, targetPos, angle);

            Rigidbody rigidBody = bullet.GetComponent<Rigidbody>();             // 投げ出す弾のrigidbodyを変数に格納します
            rigidBody.AddForce(calVelocity * rigidBody.mass, ForceMode.Impulse);     //計算された強さの力を、弾に加えます
            Destroy(bullet, 3.0f);                           // 3秒後に作られた弾を壊します

            ClickVisible();     // プレイヤーの場所にマーカーを出すメソッドに飛びます
        }
    }
    private Vector3 CalculateVelocity(Vector3 posA, Vector3 posB, float angle)
    {
        float rad = angle * Mathf.PI / 180;  // 射出角をラジアンに変換

        // 水平方向の距離xを2点間から求めます
        float x = Vector2.Distance(new Vector2(posA.x, posA.z), new Vector2(posB.x, posB.z));
        float y = posA.y - posB.y;    // 垂直方向の距離yを2点間から求めます

        // 斜方投射を初速度について計算します
        float velo = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(velo))  // 条件を満たすveloの値（初速）がなければ、（もしくは1つでなくて、たくさんあれば）・・
        {
            return Vector3.zero;  //Vector3.zeroにします
        }
        else          //それを満たす初速値があれば・・
        {
            // 方向ベクトルに速度veloをかけた値を返します
            return (new Vector3(posB.x - posA.x, x * Mathf.Tan(rad), posB.z - posA.z).normalized * velo);
        }
    }
    public void ClickVisible()
    {
        //　現在のプレイヤーの位置を取得します
        cursorMaker.transform.position = new Vector3(player.transform.position.x, 0.27f, player.transform.position.z);
        cursorMaker.SetActive(true);            //その場所に、マーカーの絵を表示します
        StartCoroutine("ClickInvisible");       //コルーチンを使います、ClickInvisible()メソッドに飛びます
    }

    IEnumerator ClickInvisible()             //コルーチンで、ClickInvisible()メソッドをセットします
    {
        yield return new WaitForSeconds(3.5f);       //コルーチンを使います、3.5秒待ちます
        cursorMaker.SetActive(false);           //マーカーの絵を非表示にします
    }
}
