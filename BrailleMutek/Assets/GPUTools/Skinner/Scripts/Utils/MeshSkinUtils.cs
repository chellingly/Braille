using UnityEngine;

namespace GPUTools.Skinner.Scripts.Utils
{
    public class MeshSkinUtils
    {
        public static Matrix4x4 CreateToWorldMatrix(SkinnedMeshRenderer skin)
        {
            var mesh = skin.sharedMesh;
            var weight = mesh.boneWeights[0];

            var bm0 = skin.bones[weight.boneIndex0].localToWorldMatrix*mesh.bindposes[weight.boneIndex0];
            var bm1 = skin.bones[weight.boneIndex1].localToWorldMatrix*mesh.bindposes[weight.boneIndex1];
            var bm2 = skin.bones[weight.boneIndex2].localToWorldMatrix*mesh.bindposes[weight.boneIndex2];
            var bm3 = skin.bones[weight.boneIndex3].localToWorldMatrix*mesh.bindposes[weight.boneIndex3];

            var matrix = new Matrix4x4();

            for (var n = 0; n < 16; n++)
            {
                matrix[n] =
                    bm0[n] * weight.weight0 +
                    bm1[n] * weight.weight1 +
                    bm2[n] * weight.weight2 +
                    bm3[n] * weight.weight3;
            }

            return matrix;
        }

        public static Matrix4x4[] CreateToWorldMatrices(SkinnedMeshRenderer skin)
        {
            var matrices = new Matrix4x4[skin.sharedMesh.vertexCount];
            CreateToWorldMatrices(skin, matrices);
            return matrices;
        }

        public static void CreateToWorldMatrices(SkinnedMeshRenderer skin, Matrix4x4[] matrices)
        {
            var mesh = skin.sharedMesh;

            var boneMatrices = new Matrix4x4[skin.bones.Length];
            for (var i = 0; i < boneMatrices.Length; i++)
                boneMatrices[i] = skin.bones[i].localToWorldMatrix * mesh.bindposes[i];

            for (var i = 0; i < mesh.vertexCount; i++)
            {
                var weight = mesh.boneWeights[i];

                var bm0 = boneMatrices[weight.boneIndex0];
                var bm1 = boneMatrices[weight.boneIndex1];
                var bm2 = boneMatrices[weight.boneIndex2];
                var bm3 = boneMatrices[weight.boneIndex3];

                var matrix = new Matrix4x4();

                for (int n = 0; n < 16; n++)
                {
                    matrix[n] =
                        bm0[n] * weight.weight0 +
                        bm1[n] * weight.weight1 +
                        bm2[n] * weight.weight2 +
                        bm3[n] * weight.weight3;
                }

                matrices[i] = matrix;
            }

        }

        public static Matrix4x4[] CreateToObjectMatrices(SkinnedMeshRenderer skin)
        {
            var matrices = CreateToWorldMatrices(skin);

            for (var i = 0; i < matrices.Length; i++)
            {
                matrices[i] = matrices[i].inverse;
            }

            return matrices;
        }
    }
}
