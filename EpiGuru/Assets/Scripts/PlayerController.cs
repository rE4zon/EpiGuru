using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private TextMeshProUGUI coinCountText; 
    [SerializeField] private GameController gameManager; 

    private int coinCount = 0;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); 

            if (touch.position.x < Screen.width / 2)
            {
                MoveLeft();
            }
            else if (touch.position.x >= Screen.width / 2)
            {
                MoveRight();
            }
        }

        MoveForward();
    }

    void MoveLeft()
    {
        float newPosition = transform.position.x - speed * Time.deltaTime;
        newPosition = Mathf.Clamp(newPosition, -5.0f, 5.0f);
        transform.position = new Vector3(newPosition, transform.position.y, transform.position.z);
    }

    void MoveRight()
    {
        float newPosition = transform.position.x + speed * Time.deltaTime;
        newPosition = Mathf.Clamp(newPosition, -5.0f, 5.0f);
        transform.position = new Vector3(newPosition, transform.position.y, transform.position.z);
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            CollectCoin(other.gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    void CollectCoin(GameObject coin)
    {
        coinCount++;
        UpdateCoinCountUI(); 
        Destroy(coin);
    }

    void UpdateCoinCountUI()
    {
        if (coinCountText != null)
        {
            coinCountText.text = "Coins: " + coinCount;
        }
    }

    void GameOver()
    {
        if (gameManager != null)
        {
            gameManager.GameOver(coinCount);
        }
    }
}
