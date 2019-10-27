using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutTrig : MonoBehaviour
{
	[SerializeField] private UnityEvent _tuttrig;

	[SerializeField] private bool alreadyBeHere;

	private void Start()
	{
		if (PlayerPrefs.GetInt("TutorFinished") == 0)
		{
			alreadyBeHere = false;
		}
		else
		{
			alreadyBeHere = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && !other.isTrigger && PlayerPrefs.GetInt("TutorFinished") == 0 && !alreadyBeHere)
		{
			alreadyBeHere = true;
			_tuttrig.Invoke();
			
		}
			
	}
}
