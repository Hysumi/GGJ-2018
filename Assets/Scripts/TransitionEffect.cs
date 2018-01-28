using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class TransitionEffect : MonoBehaviour {

	public float distOffset;
	public Shader transitionShader;
	private Material material;

	// Use this for initialization
	void Start () {
		material = new Material(transitionShader);
	}
	
	void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		material.SetFloat("_Offset", distOffset);
		Graphics.Blit(source, dest, material);
		
	}
}
