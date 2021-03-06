﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GrayBlurEffect : PostEffectBase
{
	public Texture2D vignetteTexture;

	public float blurRadius = 1.0f;
	public int downSample = 2;
	public int iteration = 1;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (MyMaterial)
		{
			if (vignetteTexture)
			{
				MyMaterial.SetTexture("_VignetteTex", vignetteTexture);
			}

			var rt1 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0, source.format);
			var rt2 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0, source.format);
			Graphics.Blit(source, rt1);
			for (int i = 0; i < iteration; i++)
			{
				MyMaterial.SetVector("_offsets", new Vector4(0, blurRadius));
				Graphics.Blit(rt1, rt2, MyMaterial);
				MyMaterial.SetVector("_offsets", new Vector4(blurRadius, 0));
				Graphics.Blit(rt2, rt1, MyMaterial);
			}
			Graphics.Blit(rt1, destination);
			RenderTexture.ReleaseTemporary(rt1);
			RenderTexture.ReleaseTemporary(rt2);
		}
		else
		{
			Graphics.Blit(source, destination);
		}
	}
}
