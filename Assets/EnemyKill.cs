using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKill : MonoBehaviour
{
    public ParticleSystem explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)       //　OnCollisionEnterメソッドで当たり判定を見ます
    {

        if (other.gameObject.tag == "MyBullet")        //　もし、当たった相手のtagが”Player”ならば・・
        {
            GameObject.Find("GameControl").GetComponent<AC>().SE(0);
            explosion.transform.position = other.transform.position;        //　パーティクルの出現位置に、そのotherの位置を入れます
            explosion.Play();
            this.gameObject.SetActive(false);


        }

    }
}
