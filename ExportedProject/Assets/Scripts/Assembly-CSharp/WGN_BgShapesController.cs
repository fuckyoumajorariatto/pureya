using UnityEngine;

public class WGN_BgShapesController : MonoBehaviour
{
	public Transform[] squareShapesTR1;

	public Transform[] doubleShapesTR1;

	public SpriteRenderer[] doubleShapesSR1;

	public Transform[] squareShapesTR2;

	public Transform[] doubleShapesTR2;

	public SpriteRenderer[] doubleShapesSR2;

	private float speed;

	private void Start()
	{
		for (int i = 0; i < squareShapesTR1.Length; i++)
		{
			squareShapesTR1[i].localPosition = new Vector2(squareShapesTR1[i].position.x, Random.Range(-3f, 0.1875f));
			squareShapesTR1[i].localScale = new Vector2(Random.Range(8f, 12f), Random.Range(4f, 6f));
		}
		for (int j = 0; j < doubleShapesTR1.Length; j++)
		{
			doubleShapesTR1[j].localPosition = new Vector2(doubleShapesTR1[j].position.x, Random.Range(1.25f, 4f));
			doubleShapesTR1[j].localScale = new Vector2(Random.Range(0.75f, 1f), Random.Range(0.75f, 1f));
		}
		for (int k = 0; k < squareShapesTR2.Length; k++)
		{
			squareShapesTR2[k].localPosition = new Vector2(squareShapesTR2[k].position.x, Random.Range(1.75f, 4f));
			squareShapesTR2[k].localScale = new Vector2(Random.Range(8f, 12f), Random.Range(4f, 6f));
		}
		for (int l = 0; l < doubleShapesTR2.Length; l++)
		{
			doubleShapesTR2[l].localPosition = new Vector2(doubleShapesTR2[l].position.x, Random.Range(-1.75f, 0.5f));
			doubleShapesTR2[l].localScale = new Vector2(Random.Range(0.75f, 1f), Random.Range(0.75f, 1f));
		}
	}

	private void Update()
	{
		if (WGN_Difficulty.WGN_difficulty == null)
		{
			return;
		}
		speed = WGN_Difficulty.WGN_difficulty.speed;
		for (int i = 0; i < squareShapesTR1.Length; i++)
		{
			squareShapesTR1[i].Translate(speed * Time.deltaTime, 0f, 0f);
			if (squareShapesTR1[i].position.x > 6f)
			{
				squareShapesTR1[i].localPosition = new Vector2(-6f, Random.Range(-3f, 0.1875f));
				squareShapesTR1[i].localScale = new Vector2(Random.Range(8f, 12f), Random.Range(4f, 6f));
			}
		}
		for (int j = 0; j < doubleShapesTR1.Length; j++)
		{
			doubleShapesTR1[j].Translate(speed * Time.deltaTime, 0f, 0f);
			if (doubleShapesTR1[j].position.x > 6f)
			{
				doubleShapesTR1[j].localPosition = new Vector2(-6f, Random.Range(1.25f, 4f));
				doubleShapesTR1[j].localScale = new Vector2(Random.Range(0.75f, 1f), Random.Range(0.75f, 1f));
				if (Random.Range(0f, 1f) < 0.5f)
				{
					doubleShapesSR1[j].flipX = !doubleShapesSR1[j].flipX;
				}
			}
		}
		for (int k = 0; k < squareShapesTR2.Length; k++)
		{
			squareShapesTR2[k].Translate(speed * Time.deltaTime, 0f, 0f);
			if (squareShapesTR2[k].position.x > 6f)
			{
				squareShapesTR2[k].localPosition = new Vector2(-6f, Random.Range(1.75f, 4f));
				squareShapesTR2[k].localScale = new Vector2(Random.Range(8f, 12f), Random.Range(4f, 6f));
			}
		}
		for (int l = 0; l < doubleShapesTR2.Length; l++)
		{
			doubleShapesTR2[l].Translate(speed * Time.deltaTime, 0f, 0f);
			if (doubleShapesTR2[l].position.x > 6f)
			{
				doubleShapesTR2[l].localPosition = new Vector2(-6f, Random.Range(-1.75f, 0.5f));
				doubleShapesTR2[l].localScale = new Vector2(Random.Range(0.75f, 1f), Random.Range(0.75f, 1f));
				if (Random.Range(0f, 1f) < 0.5f)
				{
					doubleShapesSR2[l].flipX = !doubleShapesSR2[l].flipX;
				}
			}
		}
	}
}
