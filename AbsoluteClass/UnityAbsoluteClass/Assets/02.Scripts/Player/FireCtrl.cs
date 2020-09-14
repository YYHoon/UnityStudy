using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
    public Sprite[] weaponIcons;
    public Image weaponImage;
    public Image magazineImg;
    public Text magazineText;
    public int maxBullet = 10;
    public int remainingBullet = 10;
    public float reloadTime = 2.0f;
    bool isReloading = false;
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
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if(!isReloading && Input.GetMouseButtonDown(0))
        {
            --remainingBullet;
            Fire();
            if(remainingBullet == 0)
            {
                StartCoroutine(Reloading());
            }
        }
    }
    void Fire()
    {
        StartCoroutine(shake.ShakeCamera());
        // Instantiate(bullet, firePos.position, firePos.rotation);
        var _bullet = GameManager.instance.GetBullet();
        if(_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }
        cartirdge.Play();
        MuzzleFlash.Play();
        FireSfx();
        magazineImg.fillAmount = (float)remainingBullet / (float)maxBullet;
        UpdateBulletText();
    }
    void FireSfx()
    {
        var _sfx = playerSfx.fire[(int)currWeapon];
        _audio.PlayOneShot(_sfx,1.0f);
    }
    IEnumerator Reloading()
    {
        isReloading = true;
        _audio.PlayOneShot(playerSfx.reload[(int)currWeapon], 1.0f);
        yield return new WaitForSeconds(playerSfx.reload[(int)currWeapon].length + 0.3f);
        isReloading = false;
        magazineImg.fillAmount = 1.0f;
        remainingBullet = maxBullet;
        UpdateBulletText();
    }
    void UpdateBulletText()
    {
        magazineText.text = string.Format("<color==#ff0000>{0}</color>/{1}", remainingBullet, maxBullet);
    }
    public void OnChangeWeapon()
    {
        currWeapon = (WeaponType)((int)++currWeapon % 2);
        weaponImage.sprite = weaponIcons[(int)currWeapon];
    }
}
