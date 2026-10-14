using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int poolSize = 0;
    [SerializeField] private MeleeEnemy meleeEnemy = null;
    [SerializeField] private GameObject target = null;
    [SerializeField] private float spawnTime = 1.0f;//초

    private bool inRaid = false;
    private float nowTime = 0.0f;
    private MeleeEnemy[] melee = null;

    private void Awake()
    {
        if (meleeEnemy == null)
        {
            Debug.Log("Null Melee Enemy");
            return;
        }

        melee = new MeleeEnemy[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            melee[i] = Instantiate(meleeEnemy);

            melee[i].transform.SetParent(transform);
            melee[i].transform.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (!inRaid) return;
        
        nowTime += Time.deltaTime;
        if (nowTime >= spawnTime)
        {
            nowTime = 0.0f;
            CallSpawn();
        }
    }
    public void OnStart(InputAction.CallbackContext _) { inRaid = true; }
    public void OnLineSet(InputAction.CallbackContext _) { SetArray(); }

    private Vector2 SetPointRandomization()
    {
        float spawnRange = Random.Range(-9.0f, 10.0f);
        return new Vector2(spawnRange, 9.0f);
    }
    private void CallSpawn()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!(melee[i].inRaid))
            {
                melee[i].gameObject.SetActive(true);
                melee[i].Spawn(target, SetPointRandomization());
                break;
            }
        }
    }
    private void SetArray()
    {
        MeleeEnemy leader = null;
        int[] inRaidList = new int[poolSize];
        int count = 0;
        float offset = 1.0f;
        for (int i = 0; i < poolSize; i++)
        {
            if (melee[i].inRaid)
            {
                if (leader == null) leader = melee[i];
                inRaidList[count] = i;
                count++;
            }
        }
        if (leader == null) return;
        else
        {
            for(int i = 0; i < count; i++)
            {
                if (melee[inRaidList[i]] == leader) leader.Clear();
                else
                {
                    melee[inRaidList[i]].Clear();
                    if (i % 2 == 0) melee[inRaidList[i]].transform.position = new Vector2(leader.transform.position.x + (i * offset), leader.transform.position.y);
                    else melee[inRaidList[i]].transform.position = new Vector2(leader.transform.position.x - (i * offset), leader.transform.position.y);
                }
            }
        }
    }
    public GameObject GetCloseTarget(Vector3 pos)
    {
        float maxRange = 999.9f;
        int minRangeTargetIndex = -1;
        float len;
        for (int i = 0; i < poolSize; i++)
        {
            len = (pos - melee[i].transform.position).magnitude;
            if (len < maxRange && melee[i].gameObject.activeSelf)
            {
                maxRange = len;
                minRangeTargetIndex = i;
            }
        }
        if (minRangeTargetIndex == -1) return null;
        return melee[minRangeTargetIndex].gameObject;
    }
    
}
