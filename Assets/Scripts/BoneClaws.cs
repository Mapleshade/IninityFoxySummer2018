using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneClaws : MonoBehaviour
{

	public GameObject bone;
	public GameObject claw;
	public GameObject oskolki;

	private void Start()
	{
		if (PlayerPrefs.GetInt("GameFinished") == 1)
		{
			bone.SetActive(true);
			claw.SetActive(true);
			oskolki.SetActive(false);
		}
	}
}
