Shader "Custom/TransShader"
{
	SubShader
	{

		//render objects behind the portal
		ZWrite off
		//absolutely transparent
		ColorMask 0
		//Bidirectional behaviour
		Cull off


		Stencil{
			Ref 1
			WriteMask 10

			//set all pixels in the portal to 1
			Pass replace

		}

			Pass
			{
			}
	}
}

