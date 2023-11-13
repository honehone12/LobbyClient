using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using TMPro;

namespace Lobby
{
    [System.Serializable]
    public class LobbyUiComponents
    {
        public enum PanelState
        {
            BeforeJoin,
            AfterJoin
        }

        [Header("Panel")]
        [SerializeField]
        GameObject beforeJoinPanel;
        [SerializeField]
        GameObject afterJoinPanel;
        [Header("Output")]
        [SerializeField]
        TMP_Text outputText;

        public void OnAwake()
        {
            Assert.IsNotNull(beforeJoinPanel);
            Assert.IsNotNull(afterJoinPanel);
            Assert.IsNotNull(outputText);
        }

        public void SwitchPanel(PanelState state)
        {
            switch (state)
            {
                case PanelState.BeforeJoin:
                    beforeJoinPanel.SetActive(true);
                    afterJoinPanel.SetActive(false);
                    break;
                case PanelState.AfterJoin:
                    beforeJoinPanel.SetActive(false);
                    afterJoinPanel.SetActive(true);
                    break;
                default:
                    ErrorManager.Singleton.Error("unexpected value");
                    return;
            }
        }

        public void SetOutputText(string newText)
        {
            outputText.text = newText;
        }

        public void AddOutputText(string newText)
        {
            var current = outputText.text;
            current += "\n";
            outputText.text = current + newText;
        }
    }
}
