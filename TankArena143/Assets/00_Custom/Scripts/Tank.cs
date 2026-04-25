using UnityEngine;

public class Tank : MonoBehaviour {
    public int score = 0;

    public float moveSpeed = 2f;
    public float turnSpeed = 2f;
    public Transform tankFireVfx;

    private float moveIntent = 0f, turnIntent = 0f;
    private int fireCooldown = 0;

    public void move(float mi) { moveIntent = Mathf.Clamp(mi, -1f, 1f); }
    public void turn(float ti) { turnIntent = Mathf.Clamp(ti, -1f, 1f); }

    public void fire() {
        if (fireCooldown == 0){
            fireCooldown = 50;
            if (Physics.Raycast(
                tankFireVfx.position,
                tankFireVfx.forward,
                out RaycastHit hit,
                100f,
                LayerMask.GetMask("Default") | LayerMask.GetMask("Tank")
            )) tankFireVfx.localScale = new Vector3(1, 1, hit.distance);

            if (hit.collider.TryGetComponent<Tank>(out _)) score++;
        }
    }

    void FixedUpdate() {
        transform.Translate(0, 0, moveIntent * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(0, turnIntent * turnSpeed * Time.fixedDeltaTime * Mathf.Rad2Deg, 0);
        if (fireCooldown < 45) tankFireVfx.localScale = Vector3.one;
        if (fireCooldown > 0) fireCooldown--;
    }
}
