using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTurretAimingSystem : MonoBehaviour
{
    [SerializeField] private int minMaxAngle = 90;

    [SerializeField] private Vector3 addAngle = Vector3.zero;

    [SerializeField] private float aimingInertia = 0f;
    [SerializeField] private int maxAimSpeed = 150;
    [SerializeField] private float buildupToAimingInertia = 75f;
    [SerializeField] private float reductionToAimingInertia = 200f;
    //=====================================================================
    [SerializeField] private GameObject cannon;
    [SerializeField] private float cannonOffset = 1.0f;

    //모드 컨트롤
    private int modCount = 2;
    public int nowMode = 0;
    public enum Mod
    {
        Missile = 0,
        Gun = 1
    }

    private void Update()
    {
        TakeAim();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        addAngle = new Vector3(0, 0, -1 * context.ReadValue<Vector2>().x);
    }
    public void OnChangeWep(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (nowMode == modCount - 1) nowMode = 0;
            else nowMode++;
        }
    }
    private void TakeAim()
    {
        bool inRange = transform.eulerAngles.z + addAngle.z <= minMaxAngle || transform.eulerAngles.z + addAngle.z >= minMaxAngle * 3;
        if (inRange)
        {
            transform.Rotate(addAngle * aimingInertia * Time.deltaTime);
            MoveToNonPhysicsObject();
        }
        else
        {
            if (transform.eulerAngles.z <= 180) transform.rotation = Quaternion.Euler(new Vector3(0, 0, minMaxAngle));
            else transform.rotation = Quaternion.Euler(new Vector3(0, 0, -minMaxAngle));
        }
        CalAimingInertia(inRange);
    }
    private void CalAimingInertia(bool inRange)
    {
        if (inRange)
        {
            if (aimingInertia > maxAimSpeed) aimingInertia = maxAimSpeed;
            else if (addAngle == Vector3.zero && aimingInertia > 0) aimingInertia -= reductionToAimingInertia * Time.deltaTime;
            else if (addAngle == Vector3.zero && aimingInertia <= 0) aimingInertia = 0;
            else aimingInertia += buildupToAimingInertia * Time.deltaTime;
        }
        else aimingInertia = 0;
    }
    private void MoveToNonPhysicsObject()
    {
        cannon.transform.position = transform.position + (transform.up * cannonOffset);
        cannon.transform.rotation = transform.rotation;
    }
}
