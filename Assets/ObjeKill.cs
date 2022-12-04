using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;

public class ObjeKill : MonoBehaviour
{
    //public ParticleSystem explosion;			 //　パーティクルシステムを入れる変数explosionを用意します
    private Transform otherPos;          //　当たった相手の位置を入れる変数を用意します
    bool Touch=true;
    Vector3 PPos;
    public GameObject GC;
    public GameObject FinText, Button;

    void Start()
    {
        PPos = this.transform.position;
    }
    void OnTriggerEnter(Collider other)       //　OnCollisionEnterメソッドで当たり判定を見ます
    {

        if (other.gameObject.tag == "EnemyBullet")        //　もし、当たった相手のtagが”Player”ならば・・
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<BoxCollider>().enabled = false;
            GC.GetComponent<life>().lifeCount -=1;
            
            this.transform.position = PPos;
            if (GC.GetComponent<life>().lifeCount < 0)
            {
                FinText.SetActive(true);
                Button.SetActive(true);
            }
            else
            {
                StartCoroutine("Resporn");
            }
           
            
        }

    }
    public IEnumerator Resporn()
    {

        yield return new WaitForSeconds(1);//何秒待つのか
        this.GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<BoxCollider>().enabled = true;

    }

}
