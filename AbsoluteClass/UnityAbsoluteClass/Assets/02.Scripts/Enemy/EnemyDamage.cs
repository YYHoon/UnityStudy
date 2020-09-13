using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    const string bulletTag = "BULLET";
    float hp = 100.0f;
    GameObject bloodEffect;
    // Start is called before the first frame update
    void Start()
    {
        bloodEffect = Resources.Load<GameObject>("BulletImpactFleshBigEffect");
    }
    private void OnCollisionEnter(Collision collision)
    {
        ShowBloodEffect(collision);
        Destroy(collision.gameObject);
        hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;
        if(hp<=0.0f)
        {
            GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
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
