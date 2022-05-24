using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int healty = 200;
    [Header("Player Audio")]
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] AudioClip damageSFX;
    [SerializeField] [Range(0, 1)] float deathSFXVolume = 0.75f;
    [SerializeField] [Range(0, 1)] float laserSFXVolume = 0.15f;
    [SerializeField] [Range(0, 1)] float damageSFXVolume = 0.15f;
    [Header("Projectile")]
    [SerializeField] GameObject Laser;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] float WfF = 0.3f;

    float minX;
    float maxX;
    float minY;
    float maxY;

    void Start()
    {
        SetUpBoundaries();
    }

    void Update()
    {
        Move();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }

        if(other.gameObject.tag != "Proj")
        {
            ProcessHit(damageDealer);
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        healty -= damageDealer.GetDamage();
        AudioSource.PlayClipAtPoint(damageSFX, Camera.main.transform.position, damageSFXVolume);
        damageDealer.Hit();
        if (healty <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<LevelLoader>().LoadTheEnd();
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(WaitForFire());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopAllCoroutines();
        }
    }

    IEnumerator WaitForFire()
    {
        while (true)
        {
            GameObject laser = Instantiate(Laser, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserSFXVolume);
            yield return new WaitForSeconds(WfF);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, minX, maxX);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, minY, maxY);
        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpBoundaries()
    {
        Camera gameCamera = Camera.main;
        minX = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        maxX = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        minY = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        maxY = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    public int GetHealth()
    {
        return healty;
    }

} 
