

using UnityEngine;



public class HotMissile : MonoBehaviour
{
    [SerializeField] private float thrust = 0f;
    [SerializeField] private float nonTrackRange = 0.0f;
    [SerializeField] private float trunPower = 0.0f;
    [SerializeField] private float maxG = 0f;
    [SerializeField] private EnemyManager getTarget = null;

    private GameObject target;
    private GameObject selfLauncher;
    private int lifeTime = 60;//초
    private float nowTime = 0.0f;

    private SpriteRenderer sprite;
    Rigidbody2D body2D;
    Collider2D physics2D;

    //==
    public int damage = 0;


    private bool inFly = false;
    public bool inReady = false;



    private void Awake() //기초 컴포넌트 확인, 생성과 동시에 비활성
    {
        body2D = GetComponent<Rigidbody2D>();
        physics2D = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();

        inReady = true;
        gameObject.SetActive(false);
    }
    private void FixedUpdate()
    {
        nowTime += Time.fixedDeltaTime;

        if (inFly && nowTime >= lifeTime || (transform.position.x > 100.0f || transform.position.x < -100.0f) || (transform.position.y > 100.0f || transform.position.y < -100.0f))
        {
            Clear();
            gameObject.SetActive(false);
            return;
        }
        if (target == null || target.activeSelf) getTarget.GetCloseTarget(transform.position);

        if ((selfLauncher.transform.position - transform.position).magnitude >= nonTrackRange) body2D.MoveRotation(CalTrackAngle());

        body2D.linearVelocity = transform.up * thrust;
    }
    private float CalTrackAngle()
    {
        if (target == null) return transform.eulerAngles.z;

        float angle = GetTargetAngle();

        float localAngle = GetLocalAngle();

        float G = nowTime * trunPower;
        if (G >= maxG) G = maxG;

        float tarckAngle = Mathf.LerpAngle(localAngle, angle, G * Time.fixedDeltaTime) ;

        return tarckAngle;
    }
    private float GetTargetAngle()
    {
        Vector3 vec = target.transform.position - transform.position;
        float radian = Mathf.Atan2(vec.y, vec.x);
        float angle = radian * Mathf.Rad2Deg;
        return angle - 90;
    }
    private float GetLocalAngle()
    {
        float localAngle;
        if (transform.eulerAngles.z > 180) localAngle = transform.eulerAngles.z - 360.0f;
        else localAngle = transform.eulerAngles.z;
        return localAngle;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<MeleeEnemy>())
        {
            Clear();
            gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Graund"))
        {
            Clear();
            gameObject.SetActive(false);
        }
    }
    public void SetLauncher(GameObject launcher) { selfLauncher = launcher; }
    public void Fire(GameObject target)
    {
        if (!inFly)
        {
            Clear();
            this.target = target;

            inFly = true;
            inReady = false;
        }
    }
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


        target = null;

        nowTime = 0.0f;

        inFly = false;
        inReady = true;
    }
}
