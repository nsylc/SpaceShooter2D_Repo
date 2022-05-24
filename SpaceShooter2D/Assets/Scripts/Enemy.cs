using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] int scoreValue = 150;
    [SerializeField] float healty = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject LaserBean;
    [Header("Enemy FXs")]
    [SerializeField] float LaserBeanSpeed = 10f;
    [SerializeField] GameObject ExpolosionParticles;
    [SerializeField] float timeForVFX = 1f;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] [Range(0, 1)] float deathSFXVolume = 0.75f;
    [SerializeField] [Range(0, 1)] float laserSFXVolume = 0.15f;

    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject Laser = Instantiate(LaserBean, transform.position, Quaternion.identity) as GameObject;
        Laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -LaserBeanSpeed);
        AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserSFXVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; }

        if (other.gameObject.tag != "EnemyProj")
        {
            ProcessHit(damageDealer);
        }
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        healty -= damageDealer.GetDamage();
        damageDealer.Hit();
        if(healty <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(ExpolosionParticles, transform.position, transform.rotation) as GameObject;
        Destroy(explosion, timeForVFX);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
    }
}
