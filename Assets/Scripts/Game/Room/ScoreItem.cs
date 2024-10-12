using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    [SerializeField] private int scoreValue = 100;
    [SerializeField] private ParticleSystem scoreParticle;
    [SerializeField] private FloatNumber scoreFloatnumber;
    [SerializeField] private Color scoreColor;
    [SerializeField] private AudioClip scoreClip;

    private bool hasTriggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasTriggered)
        {
            FindFirstObjectByType<PlayerCallbacks>().ScoreFeedback();
            hasTriggered = true;
            SpawnFloatNumber();
            Instantiate(scoreParticle, transform.position, Quaternion.identity);
            SoundManager.PlayAudioClip(scoreClip);
            GameController.instance.AddScore(scoreValue);
            FindFirstObjectByType<PlayerController>().AddShield(scoreValue);
            Destroy(gameObject);
        }
    }

    private void SpawnFloatNumber()
    {
        FloatNumber floatNum = Instantiate(scoreFloatnumber, transform.position, Quaternion.identity);
        floatNum.InitFloatNumber(scoreValue, scoreColor);
    }
}
