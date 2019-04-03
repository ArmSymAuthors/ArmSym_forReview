using System.Collections; 
using UnityEngine; 
using Assets.LSL4Unity.Scripts.AbstractInlets;

/// <summary>
/// IMPORTANT NOTE FOR ARMSYM:
/// This script was first written by the creators of LSL4Unity. We modified it so that it would be able to work with ArmSym. It currently works with a single pre-allocated float named "signal", which takes the value of the incoming pipe, 
/// and can be called and summoned from another script in ArmSym.
/// Refer to 'Armsym_Controlmode' for more information. 
///
/// </summary>


namespace Assets.LSL4Unity.Scripts.Examples {

    /// <summary>
    /// Original developers:
    /// Example that works with the Resolver component.
    /// This script waits for the resolver to resolve a Stream which matches the Name and Type.
    /// See the base class for more details. 
    /// 
    /// The specific implementation should only deal with the moment when the samples need to be pulled
    /// and how they should processed in your game logic
    ///
    /// </summary>
    public class DemoInletForFloatSamples : InletFloatSamples
    {
        public Transform targetTransform;

      
        public GameObject MyGameObject;
        private bool pullSamplesContinuously = false;


        public float signal;

        void Start()
        {
            // [optional] call this only, if your gameobject hosting this component
            // got instantiated during runtime
            
            // registerAndLookUpStream();
            signal = 0;
        }

        protected override bool isTheExpected(LSLStreamInfoWrapper stream)
        {
            // the base implementation just checks for stream name and type
            var predicate = base.isTheExpected(stream);
            // add a more specific description for your stream here specifying hostname etc.
            //predicate &= stream.HostName.Equals("Expected Hostname");
            return predicate;
        }

        /// <summary>
        /// Override this method to implement whatever should happen with the samples...
        /// IMPORTANT: Avoid heavy processing logic within this method, update a state and use
        /// coroutines for more complexe processing tasks to distribute processing time over
        /// several frames
        /// </summary>
        /// <param name="newSample"></param>
        /// <param name="timeStamp"></param>
        protected override void Process(float[] newSample, double timeStamp)
        {
            signal = newSample[0];



        }

        protected override void OnStreamAvailable()
        {
            pullSamplesContinuously = true;
        }

        protected override void OnStreamLost()
        {
            pullSamplesContinuously = false;
        }
         
        private void Update()
        {
            if(pullSamplesContinuously)
                pullSamples();
        }
    }
}