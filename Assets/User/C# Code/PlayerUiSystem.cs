using UnityEngine;

public class PlayerUiSystem : MonoBehaviour
{
    [SerializeField] private MeleeEnemy enemy = null;
    [SerializeField] private int maxHealth = 0;
    [SerializeField] private GameObject cannon;
    [SerializeField]private float iTime = 0.2f;
    [SerializeField] private PlayerTurretAimingSystem player = null;
    [SerializeField] private SpriteRenderer coolUi = null;
    private float nowTime;

    private SpriteRenderer sprite;
    private int hp = 0;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        hp = maxHealth;
        nowTime = 0;
    }
    private void Update()
    {
        nowTime += Time.deltaTime;
        switch(player.nowMode)
        {
            case (int)PlayerTurretAimingSystem.Mod.Missile:
                MsledColor();
                break;
            case (int)PlayerTurretAimingSystem.Mod.Gun:
                GunColor();
                break;

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MeleeEnemy>())
        {
            if (nowTime > iTime)
            {
                hp -= enemy.damage;
                SetHpColor();
            }
            else
                nowTime = 0.0f;
            if (hp < 0)
            {
                transform.rotation = Quaternion.identity;
                gameObject.SetActive(false);
                cannon.SetActive(false);
            }
        }
    }
    private void SetHpColor(){ sprite.color = new Color(1 - ((float)hp / maxHealth), (float)hp / maxHealth, 0.0f, 1.0f); }

    //무기 타입 표시자
    public void MsledColor() { coolUi.color = new Color(1.0f, 0.0f, 0.0f, 1.0f); }
    public void GunColor() { coolUi.color = new Color(0.0f, 0.0f, 1.0f, 1.0f); }
}
