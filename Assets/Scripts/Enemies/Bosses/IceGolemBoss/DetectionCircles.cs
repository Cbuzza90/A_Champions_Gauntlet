using UnityEngine;

public class DetectionCircles : MonoBehaviour
{
    public CircleCollider2D innerCircle;
    public CircleCollider2D outerCircle;
    private Transform player;
    private IceGolemBossController bossController;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossController = GetComponent<IceGolemBossController>();
    }

    void Update()
    {
        CheckPlayerPosition();
    }

    void CheckPlayerPosition()
    {
        bool isPlayerWithinRange = innerCircle.OverlapPoint(player.position) && !outerCircle.OverlapPoint(player.position);
        bossController.StayWithinRangeStateEnabled(isPlayerWithinRange);
    }
}
