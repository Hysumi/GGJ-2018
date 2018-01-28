using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class TransitionEffect : MonoBehaviour {
	[Range(-0.075f, 0.1f)]
	public float distOffset;
	[Range(0,1)]
	public float noiseBlend;
	public Shader transitionShader;
	private Material material;

	// Use this for initialization
	void Start () {
		material = new Material(transitionShader);
	}
	
	void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		material.SetFloat("_Offset", distOffset);
		material.SetFloat("_NoiseBlend", noiseBlend);
		Graphics.Blit(source, dest, material);
		
	}
}
