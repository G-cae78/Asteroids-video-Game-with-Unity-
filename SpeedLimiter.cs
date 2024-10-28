using Unity.VisualScripting;
using UnityEngine;

public class SpeedLimiter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Rigidbody rigid;
    public float speedLimit=5f;
   private void FixedUpdate() {
    float spd= rigid.linearVelocity.magnitude;
    if(spd>speedLimit)
    rigid.linearVelocity*=speedLimit/spd;
  }
}
