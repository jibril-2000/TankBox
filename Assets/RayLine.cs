using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLine : MonoBehaviour
{
    public int rayReflect;                       　   //Rayが壁に反射する回数を入れます
    public float maxLength;                          //Lineの最大長さを入れます
    public float rayBounce;
    private LineRenderer lineRenderer;　　//lineRendererを格納する変数を用意します
    private Ray ray;                                     　   //Ray型の変数を用意します
    private RaycastHit hit;                             //Rayの当たったオブジェクトの変数を用意
    private bool rayOn;                                     // rayを表示するか、表示しないかのフラグです


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();   　　　　　　　// Linerender型の変数にLineRedererコンポネントを入れます
        rayOn = gameObject.GetComponent<CHeadDir>().raylineOn;            //rayを表示するフラグraylineOnを「CHeadDirクラス」から取ってきます
    }
    void Update()
    {
        rayOn = gameObject.GetComponent<CHeadDir>().raylineOn;               //　rayを表示するフラグをCHeadDirから取ってきます

        if (rayOn == true)				　　　　　 //　もしrayOnフラグがtrueならば・・
        {
            ray = new Ray(transform.position, transform.forward);   　　　　    //　Rayの制作・・新しくこのオブジェクトから、z軸方向にRayを出します
            lineRenderer.positionCount = 1;                                 　　　　　　      //　lineRenderer変数のpositionCount（頂点の数）に1を入れます
            lineRenderer.SetPosition(0, transform.position);  　　　　　　　　//　lineRenderer関数で0地点からプレイヤーの位置まで線を描きます
            float leftLength = maxLength;                                           　　　　　　//　変数leftLengthに最大長さの値を入れます

            for (int i = 0; i < rayReflect; i++)                                      　　　　  　  //　変数rayBounceに入れ垂れた回数だけループさせます
            {
                ////もし、RaycastがObjectに当たったら・・（Rayの開始点Vector3、Rayの方向Vector3、ヒットしたオブジェクトの情報、rayの長さ）
                if (Physics.Raycast(ray.origin, ray.direction, out hit, leftLength))
                {
                    lineRenderer.positionCount += 1;                      　      　　　　//　lineRendererのpositionCountに1を足します
                    {
                        //　当たった頂点から１つ前の位置から、当たった場所まで線を引きます　
                        lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);

                        //　最大長さからrayの発車された場所から当たった場所までの距離を引きます
                        leftLength -= Vector3.Distance(ray.origin, hit.point);

                        //　変数rayに、新しく当たった場所から、反射した方向に出すrayを入れます（Reflect関数は「反射」を計算してくれます）
                        ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal)); if (hit.collider.tag != "Wall") 	 //　もし当たったcolliderのtagが”Wall”　でなければ　(反射させたい壁にWallというtagをセットします)
                        {
                            break;                                  //終わります
                        }
                        else            　　　　　　//もしくは・・　　　　
                        {
                            //壁に当たっていればlineRenderer変数のpositionCount（頂点の数）に1を足します
                            lineRenderer.positionCount += 1;

                            //ひとつ前の頂点から、rayのはじまりの地点から残りのrayの長さを足した方向へ線を描きます
                            lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * leftLength);
                        }
                    }
                }
            }
        }
        else if (rayOn == false)	　　　　　　　　　//　rayOnがfalseならば・・・
        {
            lineRenderer.positionCount = 0;                                       //lineRenderer変数のpositionCount（頂点の数）に0を入れます
        }
    }
}
