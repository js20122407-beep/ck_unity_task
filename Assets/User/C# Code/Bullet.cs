using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.WSA;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 0f;
    private GameObject selfLauncher = null;
    private Vector3 direction;
    public int damage = 0;

    private SpriteRenderer sprite;
    Collider2D physics2D;
    public bool inReady = true;
    private void Awake()
    {
        physics2D = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();

        inReady = true;
        gameObject.SetActive(false);
    }
    private void Update()
    {
        if ((transform.position.x > 100.0f || transform.position.x < -100.0f) || (transform.position.y > 100.0f || transform.position.y < -100.0f))
        {
            Clear();
            gameObject.SetActive(false);
        }
        transform.position += transform.up * speed * Time.deltaTime;
    }
    public void Fire()
    {
        transform.rotation = selfLauncher.transform.rotation;
        inReady = false;
        gameObject.SetActive(true);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MeleeEnemy>())
        {
            Clear();
            gameObject.SetActive(false);
        }
    }
    public void SetLauncher(GameObject launcher) { selfLauncher = launcher; }

    public void Clear()
    {
        if (selfLauncher != null)
        {
            transform.position = selfLauncher.transform.position;
            transform.rotation = selfLauncher.transform.rotation;
        }
        else
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        inReady = true;
    }
}
