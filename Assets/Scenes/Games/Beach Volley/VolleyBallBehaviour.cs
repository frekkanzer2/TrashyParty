using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyBallBehaviour : MonoBehaviour
{

    private static float CONST_STARTING_TIMELEFT = 20f;

    public Rigidbody2D rb2D;
    void FixedUpdate() { if (rb2D.velocity.magnitude > 80) { rb2D.velocity = rb2D.velocity.normalized * 80; } }

    float timeLeft = CONST_STARTING_TIMELEFT;
    bool _ready = false;

    public bool Ready { get { return _ready; } set { _ready = value; } }

    void Update()
    {
        if (!_ready)
        {
            timeLeft = CONST_STARTING_TIMELEFT;
            return;
        }
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Vector2 generatedForce = Vector2.zero;
            while (generatedForce.x < 3f && generatedForce.x > -3f)
                generatedForce = new Vector2(Random.Range(-10f, 10f), Random.Range(-15f, -20f));
            ApplyForce(generatedForce);
            Log.Logger.Write($"Ball has not touched anyone in the latest {CONST_STARTING_TIMELEFT}s, applied force with vector [{generatedForce.x},{generatedForce.y}]");
            timeLeft = CONST_STARTING_TIMELEFT;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) timeLeft = CONST_STARTING_TIMELEFT;
    }

    public void ApplyForce(Vector2 direction) => rb2D.AddForce(direction, ForceMode2D.Impulse);

}
