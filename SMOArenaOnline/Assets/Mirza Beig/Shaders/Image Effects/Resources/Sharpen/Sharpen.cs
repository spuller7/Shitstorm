
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using System.Collections;

using MirzaBeig.Shaders.Common;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace Shaders
    {

        // =================================	
        // Classes.
        // =================================

        [ExecuteInEditMode]
        [System.Serializable]

        public class Sharpen : IEBase
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            [Range(-2.0f, 2.0f)]
            public float strength = 0.5f;

            // ...

            [Range(0.0f, 8.0f)]
            public float edgeMult = 0.2f;

            // =================================	
            // Functions.
            // =================================

            // ...

            void Awake()
            {
                shader = Shader.Find("Hidden/Mirza Beig/Image Effects/Sharpen");
            }

            // ...

            void Start()
            {

            }

            // ...

            void Update()
            {

            }

            // ...

            void OnRenderImage(RenderTexture source, RenderTexture destination)
            {
                material.SetFloat("_strength", strength);
                material.SetFloat("_edgeMult", edgeMult);

                // ...

                blit(source, destination);
            }

            // =================================	
            // End functions.
            // =================================

        }

        // =================================	
        // End namespace.
        // =================================

    }

}

// =================================	
// --END-- //
// =================================
