using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMoveDir : MonoBehaviour
{

    private Image cursorImg;                                 //�N���b�N���Ɍ����cursor�̊G������ϐ�
    Vector2 mousePos;                                           //�}�E�Xposition�̈ʒu������ϐ��iUI��2�̐����ŕ\����j
    public float visibleTime = 0.3f;                        // �J�[�\���\�����Ԃ����܂�

    public GameObject player;�@�@	// player�I�u�W�F�N�g���i�[����ϐ�player��p�ӂ��܂�
    private Vector3 forceDir;               //  �͂�������������i�[����ϐ�forceDir��p�ӂ��܂�

    public float pushPower = 0.01f;    	// �����͂�p�ӂ��܂��Ainspector����������ł���悤public�ϐ��ɂ��܂�
    Rigidbody rigidBody;                    // rigidbody�^�̕ϐ�rigidBody��p�ӂ��܂�
    Ray ray;                                    	//���C�^�̕ϐ�ray
    RaycastHit hit;                     	//RaycastHit�^�̕ϐ�hit�@�@���C�����������I�u�W�F�N�g�̏����i�[���܂�
    public Camera cam;      //�J�����^�̕ϐ�cam��p�ӂ��܂�

 

void Start()
    {
        ////CursorPop�̃X�N���v�g�i��΂����������߂�j

        //cursorImg = GameObject.Find("PopCursor").GetComponent<Image>();   // �hPopCursor�iCanvas���j�h��T���Ă����́hImage�h��ϐ�cursorImg�ɓ����
        //cursorImg.enabled = false;                                    // cursor���\���ɂ���
        player = GameObject.Find("Player");           // "Player"�Ƃ������O�̃I�u�W�F�N�g��T���ē���܂�
    }
    void FixedUpdate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);          //�@�ϐ�ray�ɃJ��������}�E�X�J�[�\���̂���ʒu�Ƀ��C���΂��܂�
        mousePos = Input.mousePosition;                                                         //�@�ϐ�mousePos�Ɍ��݂̃J�[�\���ʒu�����܂�
        if (Physics.Raycast(ray, out hit))  			�@�@   //�@�������C�������ɓ���������E�E
        {
            Vector3 hitPos = hit.point;  			�@�@�@//�@�ϐ�hit�ɓ����ꂽ�l��Vector3�^�̕ϐ�hitPos�ɓ���܂�

            Debug.Log(hitPos);�@�@�@�@�@�@�@�@�@�@�@�@�@	�@�@�@�@�@�@�@�@//�@���������ꏊ�̍��W��\������
            Debug.DrawRay(cam.transform.position, ray.direction * 50f, Color.red);   //DrawRay�iVector3�X�^�[�g���W, �����x�N�g���ƃ��C�̒����A�F �j

            ////CursorPop�̃X�N���v�g
            Vector3 playerPos = player.transform.position;       // 3D��Ԃ�Player�̈ʒu����荞�݂܂�
                                                                 //�N���b�N���ɕ\������J�[�\���|�C���g�̊G�̍��W�Ƃ��āA�}�E�X�̃J�[�\���ʒu�����܂�
           // cursorImg.GetComponent<RectTransform>().position = new Vector2(mousePos.x, mousePos.y);
            forceDir = (playerPos - hitPos).normalized;   //�v���C���[�̈ʒu���烌�C�̓��������ʒu�������āA�x�N�g�������߂܂�

            if (Input.GetMouseButton(0))  	�@�@//�@����Mouse�{�^����1��iMouseBottonDown�j�����ꂽ��
            {
                // �����x�N�g�������ix,0,z�j�ɏՌ��͂��t�����ɉ����܂��E�E�����̂��������͂O�ɂ��܂�
                player.GetComponent<Rigidbody>().AddForce(new Vector3(-forceDir.x * pushPower, 0f, -forceDir.z * pushPower), ForceMode.Impulse);


            }
            if (Input.GetMouseButton(1))  	�@�@//�@����Mouse�{�^����1��iMouseBottonDown�j�����ꂽ��
            {
                // �����x�N�g�������ix,0,z�j�ɏՌ��͂��t�����ɉ����܂��E�E�����̂��������͂O�ɂ��܂�
                player.GetComponent<Rigidbody>().velocity = Vector3.zero;


            }
        }
    }
    public void ClickVisible()          //�@ClickVisible���\�b�h�@�@�N���b�N�������ɐԂ��J�[�\����\�������郁�\�b�h�ł��@
    {
        cursorImg.enabled = true;       //�@cursorImg��true�ɂ��܂��i�\�������܂��j
        Invoke("ClickInvisible", visibleTime);   //�@visibleTime���Ԍo������ɁA�hClickinvisible�h���\�b�h�ɔ�т܂�
    }

    ////ClickInvisible�̃X�N���v�g
    public void ClickInvisible()         //�@ClickInvisible���\�b�h�@�@�N���b�N�������ɐԂ��J�[�\�����������\�b�h�ł�
    {
        cursorImg.enabled = false;      //�@cursorImg��false�ɂ��܂��i�\���������܂��j
    }
}





