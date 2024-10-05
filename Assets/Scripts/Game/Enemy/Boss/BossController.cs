using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject areaDanger;

    private void OnEnable()
    {
        MovementController.OnEndMove += SpawnDangerInPlayer;
    }

    private void OnDisable()
    {
        MovementController.OnEndMove -= SpawnDangerInPlayer; 
    }

    private void SpawnDangerInPlayer(object sender, System.EventArgs e)
    {
        Instantiate(areaDanger, player.position, Quaternion.identity);
    }
}
