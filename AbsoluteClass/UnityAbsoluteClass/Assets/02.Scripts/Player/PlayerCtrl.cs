using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
}
public class PlayerCtrl : MonoBehaviour
{
    float h = 0.0f;
    float v = 0.0f;
    float r = 0.0f;
    Transform tr;

    public float moveSpeed = 10.0f;
    public float rotSpeed = 80.0f;

    public PlayerAnim playerAnim;
    [HideInInspector]
    public Animation anim;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        anim.clip = playerAnim.idle;
        anim.Play();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");
        Vector3 dir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(dir.normalized * moveSpeed * Time.deltaTime,Space.Self);
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);
        if(v>=0.1f)
        {
            anim.CrossFade(playerAnim.runF.name, 0.3f);
        }
        else if (v <= -0.1f)
        {
            anim.CrossFade(playerAnim.runB.name, 0.3f);
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade(playerAnim.runR.name, 0.3f);
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade(playerAnim.runL.name, 0.3f);
        }
        else
        {
            anim.CrossFade(playerAnim.idle.name, 0.3f);
        }
    }
}
