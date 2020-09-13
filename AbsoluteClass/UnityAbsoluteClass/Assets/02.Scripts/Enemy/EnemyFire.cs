using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    AudioSource audio;
    Animator animator;
    Transform playerTr;
    Transform enemyTr;
    readonly int hashFire = Animator.StringToHash("Fire");
    readonly int hashReload = Animator.StringToHash("Reload");
    float nextFire = 0.0f;
    readonly float fireRate = 0.1f;
    readonly float damping = 10.0f;
    readonly float reloadTime = 2.0f;
    readonly int maxBullet = 10;
    int currButtlet = 10;
    bool isReload = false;
    WaitForSeconds wsReload;
    public bool isFire = false;
    public AudioClip fireSfx;
    public AudioClip reloadSfx;
    public GameObject Bullet;
    public Transform firepos;
    public MeshRenderer muzzleFlash;
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        wsReload = new WaitForSeconds(reloadTime);
        muzzleFlash.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isReload && isFire)
        {
            if(Time.time>=nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0.0f, 0.3f);
            }
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }
    void Fire()
    {
        animator.SetTrigger(hashFire);
        audio.PlayOneShot(fireSfx, 1.0f);
        StartCoroutine(ShowMuzzleFlash());
        isReload = (--currButtlet % maxBullet == 0);
        GameObject bullet = Instantiate(Bullet, firepos.position, firepos.rotation);
        Destroy(bullet, 3.0f);
        if(isReload)
        {
            StartCoroutine(Reloading());
        }
    }
    IEnumerator Reloading()
    {
        muzzleFlash.enabled = false;
        animator.SetTrigger(hashReload);
        audio.PlayOneShot(reloadSfx, 1.0f);
        yield return wsReload;
        currButtlet = maxBullet;
        isReload = false;
    }
    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.enabled = true;
        Quaternion rot = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1.0f, 2.0f);
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        muzzleFlash.enabled = false;
    }
}
