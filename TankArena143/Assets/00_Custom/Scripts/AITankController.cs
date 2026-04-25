using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class AITankController : MonoBehaviour {

    public bool debugAlwaysChase = false;

    public NavPoint[] patrolRoute;

    public Tank tank;
    public Renderer tankBody;
    public Tank enemy;
    public NavPoint enemyNavPoint;
    public NavPointSpawner navPointSpawner;

    public enum TankState {
        PATROL,
        CHASE,
        FIRE
    } TankState state = TankState.PATROL;

    private int patrolStep = 0;
    private float chaseTimer = 0f;

    bool canSeeEnemy() {
        return !Physics.Raycast(
            tank.transform.position + Vector3.up * 0.25f,
            (enemy.transform.position - tank.transform.position).normalized,
            out RaycastHit hit,
            Vector3.Distance(enemy.transform.position, tank.transform.position),
            LayerMask.GetMask("Default")
        );
    }

    void pathFind(NavPoint target) {
        NavPoint reachable = null;
        foreach (NavPoint np in navPointSpawner.navPoints) {
            if ((reachable == null ||
                Vector3.Distance(np.transform.position, target.transform.position) <
                Vector3.Distance(reachable.transform.position, target.transform.position)
            ) && !Physics.BoxCast(
                tank.transform.position + Vector3.up * 0.25f,
                new Vector3(1.25f, 0.1f, 0.1f),
                (np.transform.position - tank.transform.position).normalized,
                out _,
                Quaternion.LookRotation(np.transform.position - tank.transform.position),
                Vector3.Distance(np.transform.position, tank.transform.position),
                LayerMask.GetMask("Default")
            )) reachable = np;
        }
        if (reachable == null) return;
        reachable.color();
        float targetAngle = Vector3.SignedAngle(tank.transform.forward, reachable.transform.position - tank.transform.position, Vector3.up);
        tank.move(Mathf.Clamp01(1 - Mathf.Abs(targetAngle * 0.1f)));
        tank.turn(targetAngle * 0.1f);
    }

    void Update() {
        if (debugAlwaysChase) state = TankState.CHASE;

        switch (state) {
            case TankState.PATROL:
                if (canSeeEnemy()) {
                    state = TankState.FIRE;
                    break;
                }

                tankBody.material.color = Color.green;

                NavPoint patrolNavPoint = patrolRoute[patrolStep % patrolRoute.Length];
                if (Vector3.Distance(patrolNavPoint.transform.position, tank.transform.position) < 0.1) patrolStep++;
                pathFind(patrolNavPoint);

                break;

            case TankState.CHASE:
                if (canSeeEnemy() && !debugAlwaysChase) {
                    state = TankState.FIRE;
                    break;
                }

                if (chaseTimer <= 0f && !debugAlwaysChase) {
                    state = TankState.PATROL;
                    break;
                }

                tankBody.material.color = Color.red;

                pathFind(enemyNavPoint);

                chaseTimer -= Time.deltaTime;

                break;

            case TankState.FIRE:
                if (!canSeeEnemy()) {
                    state = TankState.CHASE;
                    chaseTimer = 15f;
                    break;
                }

                tankBody.material.color = Color.red;

                tank.move(0);
                float targetAngle = Vector3.SignedAngle(tank.transform.forward, enemy.transform.position - tank.transform.position, Vector3.up);
                tank.turn(targetAngle * 0.1f);
                if (Mathf.Abs(targetAngle) < 10f) tank.fire();

                break;
        }
    }
}
