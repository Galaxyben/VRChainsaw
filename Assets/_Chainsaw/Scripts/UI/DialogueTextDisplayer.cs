using TMPro;
using Unity.Tutorials.Core.Editor;
using UnityEngine;

namespace _Chainsaw.Scripts.UI
{
    public class DialogueTextDisplayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text headerText;
        [SerializeField] private TMP_Text dialogueText;

        public void ShowDialogue(string dialogue)
        {
            //Can make it look fancier, add sfx or whatever (show text slowly and things like that)
            if(!dialogue.IsNullOrEmpty())
                dialogueText.text = dialogue;
        }

        public void ShowHeader(string header)
        {
            if (!header.IsNullOrEmpty())
            {
                headerText.text = header;
                headerText.gameObject.SetActive(true);
            }
            else
            {
                headerText.gameObject.SetActive(false);
            }
        }

        public void HideHeader()
        {
            headerText.gameObject.SetActive(false);
        }
    }
}
