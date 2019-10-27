using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wear : MonoBehaviour
{

	public GameObject Cape;

	public SkinnedMeshRenderer Fox;
	
	
	
	// Use this for initialization
	void Start ()
	{
		SkinnedMeshRenderer[] renderers = Cape.GetComponentsInChildren<SkinnedMeshRenderer>();
		if (renderers.Length > 0)
		{
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in renderers)
			{
				skinnedMeshRenderer.bones = Fox.bones;
				skinnedMeshRenderer.rootBone = Fox.rootBone;
			}
		}
	}
	

}
