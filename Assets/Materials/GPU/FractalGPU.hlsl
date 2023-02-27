#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
StructuredBuffer<float4x4> _Matrices;
#endif

void ConfigureProcedural() {
#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
	unity_ObjectToWorld = _Matrices[unity_InstanceID];
#endif
}

float4 _ColorA, _ColorB; 
float4 _SequenceNumbers;

float4 GetFractalColor() {
	#if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
		//Weyl sequence https://catlikecoding.com/unity/tutorials/basics/organic-variety/#2.3
		float4 color;
		color.rgb = lerp(
			_ColorA.rgb, _ColorB.rgb,
			frac(unity_InstanceID * _SequenceNumbers.x + _SequenceNumbers.y)
		);
		color.a = lerp(
			_ColorA.a, _ColorB.a,
			frac(unity_InstanceID * _SequenceNumbers.z + _SequenceNumbers.w)
		);
		return color;
	#else
		return _ColorA;
	#endif
}

void ShaderGraphFunction_float(float3 In, out float3 Out, out float4 FractalColor) {
	Out = In;
	FractalColor = GetFractalColor();
}

void ShaderGraphFunction_half(half3 In, out half3 Out, out half4 FractalColor) {
	Out = In;
	FractalColor = GetFractalColor();
}