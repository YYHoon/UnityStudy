using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    const string bulletTag = "BULLET";
    const string enemyTag = "ENEMY";
    float initHP = 100.0f;
    public float currHp;
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    // Start is called before the first frame update
    void Start()
    {
        currHp = initHP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == bulletTag)
        {
            Destroy(other.gameObject);
            currHp -= 5.0f;
            if(currHp<=0.0f)
            {
                PlayerDie();
            }
        }
    }
    void PlayerDie()
    {
        OnPlayerDie();
       // GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
       // for(int i=0;i<enemies.Length;++i)
       // {
       //     enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
       // }
    }
    
}
