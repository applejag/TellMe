using System.Collections;
using UnityEngine;

public class AudienceJumpingScript : MonoBehaviour
{
    public float minJumpSpeed = 5f;
    public float maxJumpSpeed = 10f;
    public float gravity = 1;
    public float minJumpInterval = 5;
    public float maxJumpInterval = 30;

    private float speed;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(Random.Range(0, maxJumpInterval-minJumpInterval));
        while (true)
        {
            Jump();
            yield return new WaitForSeconds(Random.Range(minJumpInterval, maxJumpInterval));
        }
    }

    private void Jump()
    {
        var localPos = transform.localPosition;
        speed = Random.Range(minJumpSpeed, maxJumpSpeed);
        localPos.y += speed * Time.deltaTime;
        transform.localPosition = localPos;
    }

    private void Update()
    {
        var localPos = transform.localPosition;
        if (localPos.y <= 0)
        {
            return;
        }
        speed -= gravity * Time.deltaTime;
        localPos.y += speed * Time.deltaTime;
        if (localPos.y <= 0)
        {
            localPos.y = 0;
        }
        transform.localPosition = localPos;
    }
}
