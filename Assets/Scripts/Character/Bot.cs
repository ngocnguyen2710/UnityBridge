using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    public Transform target; // Điểm đích sau khi thu thập xong
    public int maxBricksToCollect = 5; // Số lượng brick mỗi lần
    private NavMeshAgent agent;
    private List<GameObject> bricksToCollect = new List<GameObject>();
    private bool returningToTarget = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(CollectBricksRoutine());
    }

    IEnumerator CollectBricksRoutine()
    {
        while (true)
        {
            if (!returningToTarget)
            {
                FindNearestBricks();

                foreach (var brick in bricksToCollect)
                {
                    if (brick != null)
                    {
                        agent.SetDestination(brick.transform.position);
                        yield return new WaitUntil(() => agent.remainingDistance <= 0.5f); // Chờ đến khi đến nơi
                    }
                }

                returningToTarget = true;
                agent.SetDestination(target.position);
                for (int i=0; i < 10; i++){
                    // wait 10 frames
                    yield return null;
                }
                while (agent.remainingDistance > 0.5f)
                {
                    yield return null;
                    if (eaten <= 0)
                    {
                        Debug.Log("Bot hết gạch! Quay lại nhặt tiếp.");
                        returningToTarget = false; // Hủy việc về đích
                        break;
                    }
                }

                returningToTarget = false;
            }

            yield return null;
        }
    }

    void FindNearestBricks()
    {
        GameObject[] allBricks = GameObject.FindGameObjectsWithTag("Brick");
        
        bricksToCollect = allBricks
        .Where(brick => 
        {
            Brick brickObject = brick.GetComponent<Brick>();
            return brickObject.GetBrickColorType() == GetCharacterTypeColor();
        })
        .OrderBy(brick => Vector3.Distance(transform.position, brick.transform.position))
        .Take(maxBricksToCollect)
        .ToList();
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bridge") && eaten > 0)
        {
            Bridge bridge = collision.gameObject.GetComponent<Bridge>();
            if ((int)bridge.GetBridgeColor() != (int)GetCharacterType())
            {
                bridge.SetBridgeColor((int)GetCharacterType());
                bridge.GetComponent<MeshRenderer>().material.color = GetCharacterTypeColor();
                Destroy(transform.GetChild(transform.childCount - 1).gameObject);
                eaten--;
            }
        }

        if (collision.gameObject.CompareTag("Finish") && isWinning == false)
        {
            MapManager.instance.WinGame();
        }
    }
}