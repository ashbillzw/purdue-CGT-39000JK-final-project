using UnityEngine;

public class NavPoint : MonoBehaviour {
    public NavPoint parent1, parent2;

    private int colored = 0;

    public void color() { colored = 100; }

    void Update() {
        if (colored > 0) {
            colored--;
            GetComponentInChildren<Renderer>().material.color = Color.green;
        }

        else if (parent1?.colored > 0 || parent2?.colored > 0)
            GetComponentInChildren<Renderer>().material.color = Color.yellow;

        else GetComponentInChildren<Renderer>().material.color = Color.black;
    }
}
