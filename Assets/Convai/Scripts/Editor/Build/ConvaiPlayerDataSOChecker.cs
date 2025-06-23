using Convai.Scripts.Runtime.PlayerStats;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Convai.Scripts.Editor.Build
{
    public class ConvaiPlayerDataSOChecker : IPreprocessBuildWithReport
    {
        #region IPreprocessBuildWithReport Members

        public int callbackOrder { get; }

        public void OnPreprocessBuild(BuildReport report)
        {
            if (Resources.Load<ConvaiPlayerDataSO>(nameof(ConvaiPlayerDataSO)) != null) return;
            ConvaiPlayerDataSO convaiPlayerDataSO = ScriptableObject.CreateInstance<ConvaiPlayerDataSO>();
            ConvaiPlayerDataSO.CreatePlayerDataSO(convaiPlayerDataSO);
        }

        #endregion
    }
}