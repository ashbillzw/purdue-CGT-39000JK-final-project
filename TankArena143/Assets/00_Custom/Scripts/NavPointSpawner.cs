using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavPointSpawner : MonoBehaviour {
    public NavPoint navPointPrefab;
    public Transform navPointBase;
    public List<NavPoint> navPoints;
    public List<NavPoint> visualNavPoints;

    [System.Serializable] public struct NavPointPair {
        public NavPoint p1, p2;
        public void Deconstruct(out NavPoint a, out NavPoint b) { a = p1; b = p2; }
    } public NavPointPair[] basePairs;

    void Start() {
        navPoints.AddRange(FindObjectsByType<NavPoint>(FindObjectsSortMode.None));
        foreach ((NavPoint p1, NavPoint p2) in basePairs) {
            for (int i = 1; i < Vector3.Distance(p1.transform.position, p2.transform.position) - 0.99f; i++) {
                Vector3 spawnPoint = p1.transform.position + (p2.transform.position - p1.transform.position).normalized * i;
                float minDistance = 100f;
                foreach (NavPoint np in navPoints) {
                    minDistance = Mathf.Min(minDistance, Vector3.Distance(spawnPoint, np.transform.position));
                    
                }
                if (minDistance > 0.99f) {
                    NavPoint np = Instantiate(navPointPrefab, navPointBase);
                    np.transform.position = spawnPoint;
                    visualNavPoints.Add(np);
                    np.parent1 = p1;
                    np.parent2 = p2;
                }
            }
        }
    }
}
