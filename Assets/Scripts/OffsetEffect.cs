using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class OffsetEffect : MonoBehaviour {
	
	public Vector2 offset;
	public Vector2 speed;
	public float wavy;
	public Shader offsetShader;
	private Material material;
	
	void Awake()
	{
		material = new Material(offsetShader);
	}
	
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (offset == Vector2.zero)
		 {
			Graphics.Blit (source, destination);
			return;
		 }
		 
		 material.SetFloat("_SpeedX", speed[0]);
		 material.SetFloat("_SpeedY", speed[1]);
		 material.SetFloat("_Wavy", wavy);
		 material.SetFloat("_OffsetX", offset[0]);
		 material.SetFloat("_OffsetY", offset[1]);
		 Graphics.Blit (source, destination, material);		
		
	}
	
	
}
