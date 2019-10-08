using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
	public Transform[] pieces;
	float pieceTimer = -1f;
	public float dissapearTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
	{
		if (pieceTimer > -1f)
		{
			if(Time.time-pieceTimer> dissapearTime)
			{
				foreach (Transform piece in pieces)
				{
					Destroy(piece.gameObject);
				}
				Destroy(gameObject);
			}
			if (Time.time - pieceTimer > 0)
				foreach (Transform piece in pieces)
				{
					piece.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, (dissapearTime - (Time.time - pieceTimer)) / dissapearTime);
				}
		}
	}
	private void OnBreak()
	{
		if (pieceTimer == -1f)
		{
			pieceTimer = Time.time;
			foreach (Transform piece in pieces)
			{
				piece.parent = transform.parent;
				piece.GetComponent<Collider2D>().isTrigger = false;
				piece.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
				piece.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-200.0f, 200.0f), Random.Range(-200.0f, 200.0f)));
			}
		}
	}
	private void OnTriggerEnter2D( Collider2D collision )
	{
		OnBreak();
	}
}
