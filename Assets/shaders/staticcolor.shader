Shader "staticcolor" 
{
	Properties
	{
		m_Color("Color", Color) = (1,1,1)
	}
	
	SubShader
	{
		//Color[m_Color]
		Pass { }
	}
}