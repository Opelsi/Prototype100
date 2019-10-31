using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public float forwardVelocity;
	private float lifeTime = 0f;
	// Start is called before the first frame update
	void Start()
	{
		lifeTime = Time.time;
		transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(forwardVelocity, 0f));
    }

    // Update is called once per frame
    void Update()
    {
		if (transform.GetComponent<Rigidbody2D>() != null && transform.GetComponent<Rigidbody2D>().gravityScale > 0f)
		{
			Vector2 v = transform.GetComponent<Rigidbody2D>().velocity;
			float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
		if (Time.time - lifeTime > 10f) Destroy(transform.gameObject);
	}
	private void OnTriggerEnter2D( Collider2D collision )
	{
		if (collision.tag == "Level"&& transform.GetComponent<Rigidbody2D>()!=null)
		{
			Destroy(transform.GetComponent<Rigidbody2D>());
		}
	}
}
