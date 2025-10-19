using Microlight.MicroBar;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DroneStatusAndCollision : MonoBehaviour
{
    const float MAX_HP = 100f;

    public float droneLife = 100f;
    [SerializeField] float damageCounter = 2f;
    [SerializeField] float minimumDamageValue = 10f;
    //Solo puede haber un choque cada segundo
    [SerializeField] float cooldownSeconds = 1f;
    [SerializeField] float bounce = 50f;
    [SerializeField] GameObject smallSmokeVFX;
    [SerializeField] GameObject largeSmokeVFX;
    [SerializeField] MicroBar punch_MicroBar;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject alertUi;


    PlayerMover3 playerMover;

    float timer;

    public bool isCollided;

    GameSceneManager gameSceneManager;
    private void Start()
    {
        gameSceneManager = FindFirstObjectByType<GameSceneManager>();
        playerMover = GetComponent<PlayerMover3>();
        if (punch_MicroBar != null) punch_MicroBar.Initialize(MAX_HP);

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (timer > 0) { return; }
        TakeDamage(collision);
        //Reinicio el timer
        timer = cooldownSeconds;
    }
    private void Update()
    {
        //Mientras el timer sea mayor a 0 sigo restando, si esta en 0 puede recibir daño
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
    private void TakeDamage(Collision collision)
    {
        float crashVelocity = collision.relativeVelocity.magnitude;
        Debug.Log("Speed: " + crashVelocity);
        //Si el daño es mayor a minimumDamageValue el jugador recibe daño
        float damage = Mathf.Round(Mathf.Max(0f, crashVelocity - minimumDamageValue) * damageCounter);
        droneLife = Mathf.Round(droneLife - damage);
        playerMover.currentSpeed = 0f;
        GetComponent<Rigidbody>().linearVelocity = -collision.contacts[0].normal * bounce;
        Debug.Log($"Dron sufre {damage} de daño, vida = {droneLife}");
        if (damage >0 && damage < 10)
        {
            isCollided = true;

            if (punch_MicroBar != null) 
            {
                punch_MicroBar.UpdateBar(droneLife, false, UpdateAnim.Damage);
            }

            Instantiate(smallSmokeVFX, transform.position, Quaternion.identity);
        }
        else if (damage >= 10) 
        {
            isCollided = true;
            if (punch_MicroBar != null)
            {
                punch_MicroBar.UpdateBar(droneLife, false, UpdateAnim.Damage);
            }

            Instantiate(largeSmokeVFX, transform.position, Quaternion.identity);
        }
        if (droneLife <= 0)
        {
            GameOver();

        }
    }

    private void GameOver()
    {
       StartCoroutine(GameOverEnumerator());
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        print("Collision Out: " + gameObject.name);
    }

    IEnumerator GameOverEnumerator()
    {
        droneLife = 0;
        playerMover.isOn = false;
        Instantiate(largeSmokeVFX, transform.position, Quaternion.identity);
        if (alertUi != null) { alertUi.gameObject.SetActive(false); }
        
        yield return new WaitForSeconds(1f);
        
        Cursor.lockState = CursorLockMode.None;

        gameOverCanvas.SetActive(true);
        Debug.Log("Game Over");
    }
}
