using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;				//�@Cinemachine���p�����܂�

public class CountItem : MonoBehaviour
{
    
    public GameObject Next,Nstage,Restart;
    public CinemachineVirtualCamera vcamera;   //�@CinemachineVirtualCamera�^�̕ϐ���p�ӂ��܂�
    public GameObject ClearObj;
    public bool last;

    // Start is called before the first frame update
    void Start()
    {
        //Next.SetActive(false);

    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)	 �@�@//�@�������g���K�[�ɓ��������ꍇ�E�E
    {
        if (other.tag == "Player")�@�@�@�@�@�@�@//�@���������Tag���hPlayer�h�g�Ȃ�΁E�E
        {
            other.gameObject.SetActive(false);
            ClearObj.SetActive(true);
            GameObject.Find("GameControl").GetComponent<AC>().SE(1);
            if (last != true)
            {
                StartCoroutine("NextStage");//�R���[�`�����g�������Ƃ���ɂ��������
            }
            else
            {

            }
        }
    }
    public IEnumerator NextStage()
    {
        this.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(3);//���b�҂̂�
        vcamera.Priority = 20;  //�ϐ�vcamera�ɓ����ꂽV�J�����̃v���C�I���e�B��20�ɂ��܂�
        ClearObj.SetActive(false);
        Next.SetActive(true);
        //GameObject.Find("Move").GetComponent<CMoveDir>().player = Next;
        Destroy(this.gameObject);
        var pos= this.transform.root.gameObject.transform.position;
        this.transform.root.gameObject.SetActive(false);
        //Nstage.transform.position = pos;
    }
}
