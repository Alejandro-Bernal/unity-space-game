using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private SpawnManager _spawnManager;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    void Start()
    {
        // take the current position = new position(0, 0, 0)
        // a vector 3
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if(_uiManager == null)
        {
            Debug.LogError("The UI MANAGER is NULL");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        // if i hit the space we have to spawn a game object
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // moving the player
        // Why does the player move so fast? Unity uses 1 meter
        // Meter unity moves a object 1 meter per 60 frames
        // thus 60 meters per second
        // we want to convert 1 meter per second
        // 1 meter per frame, convert to 1 meter per second
        // use time.deltatime in order to do this convertion in seconds 
        // Time.deltaTime = 1 second Time in seconds it took complete the last frame in the current frame, lets us use real time
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        // if player position on the y is greater than 0
        // y position = 0
        // keep y changing keep x an z the same value
        // else if position on the y is less than -3.8f
        // y pos = -3.8f
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        // Bring player back to
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }

    }

    void FireLaser()
    {
        
        _canFire = Time.time + _fireRate;
        // Debug.Log("Space Key Pressed");
        Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
      
    }

    public void Damage()
    {
        _lives--;

        if(_lives < 1)
        {
            // Communicate with the spawn manager
            _spawnManager.OnplayerDeath();
            Destroy(this.gameObject);
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
