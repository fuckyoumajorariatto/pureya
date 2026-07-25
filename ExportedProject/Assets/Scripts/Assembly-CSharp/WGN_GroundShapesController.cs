using UnityEngine;

public class WGN_GroundShapesController : MonoBehaviour
{
	public Transform[] shapesTR;

	public Transform shapeTR;

	public SpriteRenderer shapeSR;

	private void Start()
	{
		for (int i = 0; i < shapesTR.Length; i++)
		{
			if (shapesTR[i].position.x > 6f)
			{
				shapesTR[i].position = new Vector2(shapesTR[i].position.x, Random.Range(-4f, -3.5f));
				shapesTR[i].localScale = new Vector2(Random.Range(2f, 4f), 2f);
			}
		}
	}

	private void Update()
	{
		if (WGN_Difficulty.WGN_difficulty == null)
		{
			return;
		}
		for (int i = 0; i < shapesTR.Length; i++)
		{
			shapesTR[i].Translate(WGN_Difficulty.WGN_difficulty.speed * 1.25f * Time.deltaTime, 0f, 0f);
			if (shapesTR[i].position.x > 6f)
			{
				shapesTR[i].position = new Vector2(-5f, Random.Range(-4f, -3.5f));
				shapesTR[i].localScale = new Vector2(Random.Range(2f, 4f), 2f);
			}
		}
		shapeTR.Translate(WGN_Difficulty.WGN_difficulty.speed * 1.25f * Time.deltaTime, 0f, 0f);
		if (shapeTR.position.x > 6f)
		{
			shapeTR.position = new Vector2(-5f, Random.Range(-4f, -3.5f));
			shapeTR.localScale = new Vector2(Random.Range(0.25f, 0.3f), Random.Range(0.25f, 0.4f));
			if (Random.Range(0f, 1f) < 0.5f)
			{
				shapeSR.flipX = !shapeSR.flipX;
			}
		}
	}
}
