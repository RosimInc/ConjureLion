using UnityEngine;
using System.Collections;

public class BlinkingEye : MonoBehaviour
{
	public Transform PaupiereHaut;
	public Transform PaupiereBas;

	public Vector3 PaupiereHautOpenPos;
	public Vector3 PaupiereHautClosedPos;

	public Vector3 PaupiereBasOpenPos;
	public Vector3 PaupiereBasClosedPos;

	private bool _isBlinking = false;

	void Update ()
	{
		if (UnityEngine.Random.Range(0f, 1f) < 0.01f && !_isBlinking)
		{
			StartCoroutine(Blink());
		}
	}

	private IEnumerator Blink()
	{
		_isBlinking = true;

		float ratio = 0f;

		while (ratio < 1f)
		{
			ratio += Time.deltaTime / 0.1f;

			PaupiereHaut.localPosition = Vector3.Lerp(PaupiereHautOpenPos, PaupiereHautClosedPos, ratio);
			PaupiereBas.localPosition = Vector3.Lerp(PaupiereBasOpenPos, PaupiereBasClosedPos, ratio);

			yield return null;
		}

		ratio = 0f;

		while (ratio < 1f)
		{
			ratio += Time.deltaTime / 0.1f;

			PaupiereHaut.localPosition = Vector3.Lerp(PaupiereHautClosedPos, PaupiereHautOpenPos, ratio);
			PaupiereBas.localPosition = Vector3.Lerp(PaupiereBasClosedPos, PaupiereBasOpenPos, ratio);

			yield return null;
		}

		_isBlinking = false;
	}
}
