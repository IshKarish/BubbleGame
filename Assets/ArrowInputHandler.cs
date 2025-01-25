using Unity.Mathematics;
using UnityEngine;

public class ArrowInputHandler : MonoBehaviour
{
    [SerializeField] private KeyCode matchingKey;

    [SerializeField] private GameObject matchingBubbleGood;
    [SerializeField] private GameObject matchingBubblePerfect;
    
    [SerializeField] private float arrowVelocity = 2;
    
    private bool _isGood;
    private bool _isPerfect;
    private bool _useInput;
    private void Update()
    {
        if (Input.GetKeyDown(matchingKey) && _useInput)
        {
            _useInput = false;
            GetComponent<BoxCollider2D>().enabled = false;
            
            if (_isPerfect) SpawnBubble(true);
            if (_isGood) SpawnBubble(false);
            
            if (!_isGood && !_isPerfect)
            {
                Debug.Log($"Lane {matchingKey} Missed");
            }
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PerfectTrigger"))
        {
            _isPerfect = true;
            _isGood = false;
        }
        else if (other.CompareTag("GoodTrigger"))
        {
            _useInput = true;
            _isGood = true;
            _isPerfect = false;
        }
        else if (other.CompareTag("MissedTrigger"))
        {
            _isPerfect = false;
            _isGood = false;
        }
    }

    void SpawnBubble(bool perfect)
    {
        GameObject newBubble = Instantiate(perfect ? matchingBubbleGood : matchingBubblePerfect, transform.position, quaternion.identity);
        newBubble.GetComponent<Rigidbody2D>().linearVelocityX = arrowVelocity;
        
        Destroy(gameObject);
    }
}
