using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Agent{
    void moveToRandomAlcove();
    void UseTeleportTrap();
    GameObject findClosestObject(HashSet<GameObject> objects);

}
