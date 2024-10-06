using UnityEngine;

[System.Serializable]
public enum BossAttacks
{
    FireArea, Explosion, Enemy, PathFire
}

public class BossController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [Header("Attacks")]
    [SerializeField] private GameObject areaDanger;
    [SerializeField] private BossAttacks attacks;

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
        if(attacks == BossAttacks.PathFire)
        {
            Instantiate(areaDanger, player.position, Quaternion.identity);
        }
    }
}
