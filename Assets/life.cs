using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class life : MonoBehaviour
{
    public int lifeCount;
    public GameObject[] lifeObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lifeObj.Length; i++)
        {
            lifeObj[i].SetActive(false);
        }
        
        if (lifeCount >= 1)
        {
            for (int i = 0; i < lifeCount; i++)
            {
                lifeObj[i].SetActive(true);
            }
        }
    }
}
