using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class MessageBox : CSingleton<MessageBox>
{
	public enum TYPE
	{
		YesOrNo,
		Ok,
		RollMessage,
	}
	
	public void OpenMessage(TYPE _type, string _title, string _content,
		UIMessageBox.CallBackFunction _yes_callback = null, UIMessageBox.CallBackFunction _no_callback = null)
	{
		if(_type == TYPE.YesOrNo || _type == TYPE.Ok)
		{
			UIMessageBox ui_box = MenuManager.instance.CreateMenu<UIMessageBox>();
			ui_box.OpenScreen();

			ui_box.mTitle.text = TextManager.instance.GetText(_title);
			ui_box.mContent.text = TextManager.instance.GetText(_content);
			ui_box.mYesCallback = _yes_callback;
			ui_box.mNoCallback = _no_callback;

			if(_type == TYPE.YesOrNo)
			{
				ui_box.mBtnYes.gameObject.SetActive(true);
				ui_box.mBtnNo.gameObject.SetActive(true);
				ui_box.mBtnOK.gameObject.SetActive(false);
			}
			else if(_type == TYPE.Ok)
			{
				ui_box.mBtnYes.gameObject.SetActive(false);
				ui_box.mBtnNo.gameObject.SetActive(false);
				ui_box.mBtnOK.gameObject.SetActive(true);
			}
		}
		else if(_type == TYPE.RollMessage)
		{
			UIRollMessageBox roll_message_ui = MenuManager.instance.CreateMenu<UIRollMessageBox>();
			roll_message_ui.OpenScreen();

			roll_message_ui.SetMessage(TextManager.instance.GetText(_content));
		}
	}

    public void OpenMessageBoxWaiting()
    {
        UIMessageBoxWaiting mMessageBoxWaiting = MenuManager.instance.CreateMenu<UIMessageBoxWaiting>();
        mMessageBoxWaiting.OpenScreen();
    }

    public void CloseMessageBoxWaiting()
    {
        UIMessageBoxWaiting mMessageBoxWaiting = MenuManager.instance.FindMenu<UIMessageBoxWaiting>();
        mMessageBoxWaiting.CloseScreen();
    }
}