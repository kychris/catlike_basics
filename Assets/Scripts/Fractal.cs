using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Mathematics.math;
using float4x4 = Unity.Mathematics.float4x4;
using quaternion = Unity.Mathematics.quaternion;
using Random = UnityEngine.Random;


public class Fractal : MonoBehaviour
{
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    struct UpdateFractalLevelJob : IJobFor
    {
        public float scale;
        public float deltaTime;

        [ReadOnly]
        public NativeArray<FractalPart> parents;
        public NativeArray<FractalPart> parts;

        [WriteOnly]
        public NativeArray<float4x4> matrices;

        public void Execute(int i) {
            FractalPart parent = parents[i / 5];
            FractalPart part = parts[i];
            part.spinAngle += part.spinVelocity * deltaTime;

            float3 upAxis = mul(mul(parent.worldRotation, part.rotation), up());
            float3 sagAxis = cross(up(), upAxis);
            float sagMagnitude = length(sagAxis);
            quaternion baseRotation;
            if (sagMagnitude > 0f)
            {
                sagAxis /= sagMagnitude;
                quaternion sagRotation =
                    quaternion.AxisAngle(sagAxis, part.maxSagAngle * sagMagnitude);
                baseRotation = mul(sagRotation, parent.worldRotation);
            }
            else
            {
                baseRotation = parent.worldRotation;
            }

            part.worldRotation = mul(baseRotation,
                mul(part.rotation, quaternion.RotateY(part.spinAngle))
            );
            part.worldPosition =
                 parent.worldPosition +
                 //mul(parent.worldRotation, (1.5f * scale * part.direction));
                 mul(part.worldRotation, float3(0f, 1.5f * scale, 0f));
            parts[i] = part;

            matrices[i] = float4x4.TRS(
                part.worldPosition, part.worldRotation, float3(scale)
            );
        }
    }

    [SerializeField, Range(3, 8)]
    int depth = 4;

    [SerializeField]
    Mesh mesh, leafMesh;

    [SerializeField]
    Material material;

    [SerializeField]
    Gradient gradientA, gradientB;

    [SerializeField]
    Color leafColorA, leafColorB;

    [SerializeField, Range(0f, 90f)]
    float maxSagAngleA = 15f, maxSagAngleB = 25f;

    [SerializeField, Range(0f, 90f)]
    float spinSpeedA = 20f, spinSpeedB = 25f;

    [SerializeField, Range(0f, 1f)]
    float reverseSpinChance = 0.25f;

    static readonly int
        colorAId = Shader.PropertyToID("_ColorA"),
        colorBId = Shader.PropertyToID("_ColorB"), 
        matricesId = Shader.PropertyToID("_Matrices"),
        sequenceNumbersId = Shader.PropertyToID("_SequenceNumbers");



    struct FractalPart
    {
        public float3 worldPosition;
        public quaternion rotation, worldRotation;
        public float maxSagAngle, spinAngle, spinVelocity;
    }

    static quaternion[] rotations = {
        quaternion.identity,
        quaternion.RotateZ(-0.5f * PI), quaternion.RotateZ(0.5f * PI),
        quaternion.RotateX(0.5f * PI), quaternion.RotateX(-0.5f * PI)
    };

    NativeArray<FractalPart>[] parts;
    NativeArray<float4x4>[] matrices;

    ComputeBuffer[] matricesBuffers;

    static MaterialPropertyBlock propertyBlock;
    	
    Vector4[] sequenceNumbers;


    private void OnEnable()
    {
        parts = new NativeArray<FractalPart>[depth];
        matrices = new NativeArray<float4x4>[depth];
        matricesBuffers = new ComputeBuffer[depth];

        sequenceNumbers = new Vector4[depth];
        int stride = 16 * 4;
        for (int i = 0, length = 1; i < parts.Length; i++, length *= 5)
        {
            parts[i] = new NativeArray<FractalPart>(length, Allocator.Persistent);
            matrices[i] = new NativeArray<float4x4>(length, Allocator.Persistent);
            matricesBuffers[i] = new ComputeBuffer(length, stride);
            sequenceNumbers[i] = new Vector4(Random.value, Random.value, Random.value, Random.value);
        }

        parts[0][0] = CreatePart(0);

        for (int li = 1; li < parts.Length; li++)
        {
            NativeArray<FractalPart> levelParts = parts[li];
            for (int fpi = 0; fpi < levelParts.Length; fpi += 5)
            {
                for (int ci = 0; ci < 5; ci++)
                {
                    levelParts[fpi + ci] = CreatePart(ci);
                }
            }
        }

        propertyBlock ??= new MaterialPropertyBlock();
    }
    void OnDisable()
    {
        for (int i = 0; i < matricesBuffers.Length; i++)
        {
            matricesBuffers[i].Release();
            parts[i].Dispose();
            matrices[i].Dispose();
        }
        parts = null;
        matrices = null;
        matricesBuffers = null;
        sequenceNumbers = null;
    }
    void OnValidate()
    {
        if (parts != null && enabled)
        {
            OnDisable();
            OnEnable();
        }
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        FractalPart rootPart = parts[0][0];
        rootPart.spinAngle += rootPart.spinVelocity * deltaTime;

        rootPart.worldRotation = mul(transform.rotation,
            mul(rootPart.rotation, quaternion.RotateY(rootPart.spinAngle))
        );
        rootPart.worldPosition = transform.position;
        parts[0][0] = rootPart;
        float objectScale = transform.lossyScale.x;
        matrices[0][0] = float4x4.TRS(
            rootPart.worldPosition, rootPart.worldRotation, float3(objectScale)
        );

        float scale = objectScale;

        JobHandle jobHandle = default;
        for (int li = 1; li < parts.Length; li++)
        {
            scale *= 0.5f;
            var job = new UpdateFractalLevelJob
            {
                deltaTime = deltaTime,
                scale = scale,
                parents = parts[li - 1],
                parts = parts[li],
                matrices = matrices[li]
            };
            jobHandle = job.ScheduleParallel(parts[li].Length, 1, jobHandle); //schedule all jobs
        }
        jobHandle.Complete(); //bundle all jobs together

        var bounds = new Bounds(rootPart.worldPosition, 3f * objectScale * Vector3.one);
        int leafIndex = matricesBuffers.Length - 1;
        for (int i = 0; i < matricesBuffers.Length; i++)
        {
            ComputeBuffer buffer = matricesBuffers[i];
            buffer.SetData(matrices[i]);

            Mesh instanceMesh;
            Color colorA, colorB;
            if (i == leafIndex)
            {
                colorA = leafColorA;
                colorB = leafColorB;
                instanceMesh = leafMesh;
            }
            else
            {
                float gradientInterpolator = i / (matricesBuffers.Length - 2f);
                colorA = gradientA.Evaluate(gradientInterpolator);
                colorB = gradientB.Evaluate(gradientInterpolator);
                instanceMesh = mesh;
            }
            propertyBlock.SetColor(colorAId, colorA);
            propertyBlock.SetColor(colorBId, colorB);

            propertyBlock.SetBuffer(matricesId, buffer);
            propertyBlock.SetVector(sequenceNumbersId, sequenceNumbers[i]);

            Graphics.DrawMeshInstancedProcedural(
                instanceMesh, 0, material, bounds, buffer.count, propertyBlock
            );
        }
    }

    FractalPart CreatePart(int childIndex) => new FractalPart
    {
        maxSagAngle = radians(Random.Range(maxSagAngleA, maxSagAngleB)),
        rotation = rotations[childIndex],
        spinVelocity =
            (Random.value < reverseSpinChance ? -1f : 1f) *
            radians(Random.Range(spinSpeedA, spinSpeedB))
    };
}