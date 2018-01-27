using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FxBlackWhite : MonoBehaviour {

	[Range(0, 1)]
	public float intensity;
	public Shader bnwShader;
	private Material material;
	
	void Awake()
	 {
		material = new Material(bnwShader);
	 }
	
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		 if (intensity == 0)
		 {
			Graphics.Blit (source, destination);
			return;
		 }
		 
		 material.SetFloat("_bwBlend", intensity);
		 Graphics.Blit (source, destination, material);			
	}
}
