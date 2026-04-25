using UnityEngine;

public class PlayerTankController : MonoBehaviour {

    public Tank tank;

    void Update() {
        int mi = 0;
        int ti = 0;

        if (Input.GetKey(KeyCode.W)) mi++;
        if (Input.GetKey(KeyCode.S)) mi--;
        if (Input.GetKey(KeyCode.A)) ti--;
        if (Input.GetKey(KeyCode.D)) ti++;

        tank.move(mi);
        tank.turn(ti);

        if (Input.GetKey(KeyCode.Space)) tank.fire();
    }
}
