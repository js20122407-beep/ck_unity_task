using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystem : MonoBehaviour
{
    [SerializeField] private GameObject launcher = null;
    [SerializeField] private PlayerTurretAimingSystem player = null;
    [SerializeField] private Bullet bullet = null;
    [SerializeField] private int poolSize = 30; 
    private Bullet[] bulletPool = null;

    private void Awake()
    {
        if (bullet == null) return;

        bulletPool = new Bullet[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            bulletPool[i] = Instantiate(bullet);
            bulletPool[i].SetLauncher(launcher);
            bulletPool[i].transform.SetParent(transform);
        }
    }
    public void OnGunFire(InputAction.CallbackContext context)
    {
        if (context.started && (player.nowMode == (int)PlayerTurretAimingSystem.Mod.Gun))
        {
            for (int i = 0; i < poolSize; i++)
            {
                if (bulletPool[i].inReady)
                {
                    bulletPool[i].gameObject.SetActive(true);
                    bulletPool[i].Clear();
                    bulletPool[i].Fire();
                    break;
                }
            }
        }
    }
}
