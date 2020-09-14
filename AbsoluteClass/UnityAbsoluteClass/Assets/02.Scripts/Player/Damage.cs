using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    const string bulletTag = "BULLET";
    const string enemyTag = "ENEMY";
    float initHP = 100.0f;
    public float currHp;
    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;
    public Image bloodScreen;
    public Image hpBar;
    readonly Color initColor = new Vector4(0, 1.0f, 0.0f, 1.0f);
    Color currColor;
    // Start is called before the first frame update
    void Start()
    {
        currHp = initHP;
        hpBar.color = initColor;
        currColor = initColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == bulletTag)
        {
            Destroy(other.gameObject);
            StartCoroutine(ShowBloodScreen());
            currHp -= 5.0f;
            DisplayHpbar();
            if (currHp<=0.0f)
            {
                PlayerDie();
            }
        }
    }
    IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(1, 0, 0, Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.1f);
        bloodScreen.color = Color.clear;
    }
    void PlayerDie()
    {
        OnPlayerDie();
        GameManager.instance.isGameOver = true;
       // GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
       // for(int i=0;i<enemies.Length;++i)
       // {
       //     enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
       // }
    }
    void DisplayHpbar()
    {
        if((currHp/initHP)>0.5f)
        {
            currColor.r = (1 - currHp / initHP) * 2.0f;
        }
        else
        {
            currColor.g = currHp / initHP * 2.0f;
        }

        hpBar.color = currColor;
        hpBar.fillAmount = currHp / initHP;
    }
}
