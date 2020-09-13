using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}
public class FireCtrl : MonoBehaviour
{
    public enum WeaponType
    {
        RIFLE =0,
        SHOTGUN
    }
    public WeaponType currWeapon = WeaponType.RIFLE;

    public GameObject bullet;
    public Transform firePos;
    public ParticleSystem cartirdge;
    private ParticleSystem MuzzleFlash;
    AudioSource _audio;
    public PlayerSfx playerSfx;
    Shake shake;
    // Start is called before the first frame update
    void Start()
    {
        MuzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
        _audio = GetComponent<AudioSource>();
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }
    void Fire()
    {
        StartCoroutine(shake.ShakeCamera());
        Instantiate(bullet, firePos.position, firePos.rotation);
        cartirdge.Play();
        MuzzleFlash.Play();
        FireSfx();
    }
    void FireSfx()
    {
        var _sfx = playerSfx.fire[(int)currWeapon];
        _audio.PlayOneShot(_sfx,1.0f);
    }
}
