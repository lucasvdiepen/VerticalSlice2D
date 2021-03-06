using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWave : MonoBehaviour
{
    private float speed;
    private bool soundWaveStarted = false;
    private Vector2 diff;

    public void StartSoundWave(Vector2 target, float speed)
    {
        this.speed = speed;
        soundWaveStarted = true;

        diff = target - new Vector2(transform.position.x, transform.position.y);
        diff.Normalize();

        float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ + 180);

        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        if(soundWaveStarted)
        {
            transform.position += new Vector3(diff.x, diff.y, 0) * speed * Time.deltaTime;
        }
    }
}
