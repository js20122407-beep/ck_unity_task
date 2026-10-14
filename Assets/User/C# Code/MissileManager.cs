using UnityEngine;
using UnityEngine.InputSystem;

public class MissileManager : MonoBehaviour
{
    [SerializeField] private GameObject launcher = null;
    [SerializeField] private int poolSize = 0;
    [SerializeField] private HotMissile hotMsl = null;
    //private GameObject  coldMsl = null;
    [SerializeField] private EnemyManager getTarget = null;
    [SerializeField] private PlayerTurretAimingSystem player = null;
    private HotMissile[] hotPool = null;

    private void Awake()
    {
        if (hotMsl == null)
        {
            Debug.Log("Null Hot MSL");
            return;
        }
        hotPool = new HotMissile[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            hotPool[i] = Instantiate(hotMsl);
            hotPool[i].SetLauncher(launcher);
            hotPool[i].transform.SetParent(transform);
        }
    }

    public void OnMslFire(InputAction.CallbackContext context)
    {
        if (context.started && (player.nowMode == (int)PlayerTurretAimingSystem.Mod.Missile))
        {
            for (int i = 0; i < poolSize; i++)
            {
                if (hotPool[i].inReady)
                {
                    hotPool[i].Clear();
                    Vector3 vec = hotPool[i].transform.position;
                    hotPool[i].Fire(getTarget.GetCloseTarget(vec));
                    hotPool[i].gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}
