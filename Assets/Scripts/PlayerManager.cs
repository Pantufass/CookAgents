using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private readonly int PLAYERNUMBER = 4;

    public List<GameObject> playerPrefabs = new List<GameObject>();

    private List<GameObject> players = new List<GameObject>();


    private List<Vector3> positions = new List<Vector3>();

    private List<Vector3> ocupiedPositions = new List<Vector3>();

    public GameObject ground;

    // Start is called before the first frame update
    void Start()
    {
        GetPositions();

        SpawnPlayers();


    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("update");
    }

    private void GetPositions()
    {
        foreach (Transform t in ground.transform)
        {
            positions.Add(new Vector3(t.position.x, t.position.y, -1));
        }
    }

    private void SpawnPlayers()
    {
        for (int i = 0; i < PLAYERNUMBER; i++)
        {
            Vector3 position = positions[Random.Range(0, positions.Count - 1)];
            while (ocupiedPositions.Contains(position))
            {
                position = positions[Random.Range(0, positions.Count - 1)];

            }
            ocupiedPositions.Add(position);

            GameObject player = Instantiate(playerPrefabs[i], position, Quaternion.identity);

            players.Add(player);
            player.GetComponent<Agent>().SetId(i + 1);
        }
        for(int j = 0; j < players.Count; j++)
        {
            for(int h = 0; h < players.Count; h++)
            {
                if(j != h)
                {
                    players[j].GetComponent<Agent>().AddOtherPlayer(players[h]);
                    players[j].GetComponent<PlayerMap>().AddOtherPlayer(players[h]);
                }
            }
        }
    }
}
