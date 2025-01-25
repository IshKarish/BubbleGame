using Unity.Mathematics;
using UnityEngine;

enum Timing
{
    Perfect, 
    Good,
    Missed
}

public class ArrowInputHandler : MonoBehaviour
{
    [SerializeField] private KeyCode matchingKey;

    [SerializeField] private GameObject matchingBubbleGood;
    [SerializeField] private GameObject matchingBubblePerfect;
    [SerializeField] private GameObject destroyedBubble;
    
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
            
            if (_isPerfect) SpawnBubble(Timing.Perfect);
            if (_isGood) SpawnBubble(Timing.Good);
            if (!_isGood && !_isPerfect) SpawnBubble(Timing.Missed);
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

    void SpawnBubble(Timing timing)
    {
        GameObject newBubble = Instantiate(GetBubbleFromTiming(timing), transform.position, quaternion.identity);
        newBubble.GetComponent<Rigidbody2D>().linearVelocityX = arrowVelocity;
        
        if (timing == Timing.Missed) Destroy(newBubble, 1);
        Destroy(gameObject);
    }

    GameObject GetBubbleFromTiming(Timing timing)
    {
        switch (timing)
        {
            case Timing.Perfect:
                return matchingBubblePerfect;
            case Timing.Good:
                return matchingBubbleGood;
            case Timing.Missed:
                return destroyedBubble;
        }

        return null;
    }
}
