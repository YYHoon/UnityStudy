using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    const string bulletTag = "BULLET";
    float hp = 100.0f;
    float initHp = 100.0f;
    GameObject bloodEffect;
    public GameObject hpBarPrefab;
    public Vector3 hpBarOffSet = new Vector3(0, 2.2f, 0);
    //public EnemyAI enemyAI;
    Canvas uiCanvas;
    Image hpBarImage;
    // Start is called before the first frame update
    void Start()
    {
       // enemyAI = GetComponent<EnemyAI>();
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
        SetHpBar();
    }
    void SetHpBar()
    {
        uiCanvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, uiCanvas.transform);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];

        var _hpBar = hpBar.GetComponent<EnemyHpBar>();
        _hpBar.targetTr = this.gameObject.transform;
        _hpBar.offset = hpBarOffSet;
    }
    private void OnCollisionEnter(Collision collision)
    {
        ShowBloodEffect(collision);
        // Destroy(collision.gameObject);
        collision.gameObject.SetActive(false);
        hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
        hpBarImage.fillAmount = hp / initHp;
        if(hp<=0.0f)
        {
            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
            hpBarImage.GetComponentsInParent<Image>()[1].color = Color.clear;
        }
    }
   
    void ShowBloodEffect(Collision collision)
    {
        Vector3 pos = collision.contacts[0].point;
        Vector3 normal = collision.contacts[0].normal;
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, normal);

        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
}
