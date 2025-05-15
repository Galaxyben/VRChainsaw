using System;
using _Chainsaw.Scripts.Tutorial;
using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public abstract class BasicStep
{
    [SerializeField] protected LocalizedString header;
    [Tooltip("Message that the UI will show when this step starts, leave as 'none' if it shouldn't change the previous step")]
    [SerializeField] protected LocalizedString message;
    
    protected TutorialDisplayUtilities displayUtils;
    
    public virtual void Initialize(TutorialDisplayUtilities displayUtilities)
    {
        displayUtils = displayUtilities;
        if (!message.IsEmpty)
        {
            displayUtils.dialogueDisplayer?.ShowDialogue(message.GetLocalizedString());
            message.StringChanged += UpdateString;
        }

        if (!header.IsEmpty)
        {
            displayUtils.dialogueDisplayer?.ShowHeader(header.GetLocalizedString());
            header.StringChanged += UpdateHeader;
        }
        else
        {
            displayUtils.dialogueDisplayer?.HideHeader();
        }
    }

    public virtual void Finish()
    {
        if (message.IsEmpty) return;
        
        message.StringChanged -= UpdateString;
        header.StringChanged -= UpdateHeader;
    }
    
    private void UpdateString(string text)
    {
        displayUtils.dialogueDisplayer?.ShowDialogue(text);
    }

    private void UpdateHeader(string text)
    {
        displayUtils.dialogueDisplayer?.ShowHeader(text);
    }
}
