using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 0;
    [SerializeField] private float MoveingSpeed = 0.0f;
    [SerializeField] private HotMissile msl = null;
    [SerializeField] private Bullet gun = null;
    private GameObject target = null;

    private Rigidbody2D body2D;
    private Collider2D physics2D;
    private SpriteRenderer sprite;
    private int hp;

    public int damage = 0;

    public bool inRaid = false;

    private void Awake()
    {
        body2D = GetComponent<Rigidbody2D>();
        physics2D = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();

        hp = maxHealth;

        inRaid = false;
    }
    private void Update()
    {
        SetHpColor();
    }
    private void FixedUpdate()
    {
        Move();
        SeeTarget();
    }
    private void Move()
    {
        body2D.linearVelocity += TrackTarget() * MoveingSpeed * Time.fixedDeltaTime;
    }
    private Vector2 TrackTarget()
    {
        if (target == null) return Vector3.zero;

        Vector2 direction = target.transform.position - transform.position;
        direction.Normalize();
        return direction;
    }
    private void SeeTarget()
    {
        if (target == null) return;

        Vector3 vec = target.transform.position - transform.position;
        float radian = Mathf.Atan2(vec.y, vec.x);
        float angle = (radian * Mathf.Rad2Deg) - 90;

        body2D.MoveRotation(angle);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<HotMissile>()) {
            hp -= msl.damage;
            if (hp < 0)
            {
                Clear();
                inRaid = false;
            }
        }
        else if (collision.gameObject.GetComponent<Bullet>())
        {
            hp -= gun.damage;
            if (hp < 0)
            {
                Clear();
                inRaid = false;
            }
        }
    }
    private void SetHpColor() { sprite.color = new Color(1 - ((float)hp / maxHealth), (float)hp / maxHealth, 0.0f, 1.0f); }
    public void Spawn(GameObject target, Vector2 pos)
    {
        this.target = target;
        body2D.linearVelocity = Vector3.zero;
        body2D.MovePosition(pos);
        inRaid = true;
    }
    public void Spawn(GameObject target, Vector2 pos, Sprite img)
            {
        this.target = target;
        body2D.linearVelocity = Vector3.zero;
        body2D.MovePosition(pos);
        sprite.sprite = img;
        inRaid = true;
    }
    public void Clear()
    {
        transform.position = new Vector2(0.0f, 9.0f);
        transform.rotation = Quaternion.identity;
        body2D.linearVelocity = Vector3.zero;
        hp = maxHealth;
    }
}
