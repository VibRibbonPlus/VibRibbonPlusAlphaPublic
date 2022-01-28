using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temprotate : MonoBehaviour
{
    public void Update() => transform.rotation = Quaternion.Lerp(transform.rotation, new Quaternion(transform.rotation.x, transform.rotation.y + 10, transform.rotation.z, transform.rotation.w), 100 * Time.deltaTime);
}
