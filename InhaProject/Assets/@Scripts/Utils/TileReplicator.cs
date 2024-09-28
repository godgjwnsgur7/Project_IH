using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileReplicator : MonoBehaviour
{
    [SerializeField] GameObject go;

    private void Reset()
    {
        go = transform.GetChild(0).gameObject;
        for (int i = 1; i < 5; i++)
        {
            GameObject temp1 = GameObject.Instantiate(go);
            GameObject temp2 = GameObject.Instantiate(go);
            temp1.transform.position = go.transform.position + (Vector3.forward * 4 * i);
            temp2.transform.position = go.transform.position + (Vector3.forward * -4 * i);
            temp1.transform.parent = this.transform;
            temp2.transform.parent = this.transform;
        }
    }
}
