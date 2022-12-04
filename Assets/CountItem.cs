using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;				//　Cinemachineを継承します

public class CountItem : MonoBehaviour
{
    
    public GameObject Next,Nstage,Restart;
    public CinemachineVirtualCamera vcamera;   //　CinemachineVirtualCamera型の変数を用意します
    public GameObject ClearObj;
    public bool last;

    // Start is called before the first frame update
    void Start()
    {
        //Next.SetActive(false);

    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)	 　　//　何かがトリガーに当たった場合・・
    {
        if (other.tag == "Player")　　　　　　　//　もしそれのTagが”Player”トならば・・
        {
            other.gameObject.SetActive(false);
            ClearObj.SetActive(true);
            GameObject.Find("GameControl").GetComponent<AC>().SE(1);
            if (last != true)
            {
                StartCoroutine("NextStage");//コルーチンを使いたいところにこれを入れる
            }
            else
            {

            }
        }
    }
    public IEnumerator NextStage()
    {
        this.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(3);//何秒待つのか
        vcamera.Priority = 20;  //変数vcameraに入れられたVカメラのプライオリティを20にします
        ClearObj.SetActive(false);
        Next.SetActive(true);
        //GameObject.Find("Move").GetComponent<CMoveDir>().player = Next;
        Destroy(this.gameObject);
        var pos= this.transform.root.gameObject.transform.position;
        this.transform.root.gameObject.SetActive(false);
        //Nstage.transform.position = pos;
    }
}
