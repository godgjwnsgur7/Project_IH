using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


enum ItemSlotData
{

}

public class UI_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField] protected Sprite slot_img;
	[SerializeField] protected Sprite front_img;

	protected UI_ToolTip uiToolTip;

	protected Image slotImage;
	public Image frontImage;

	private string slotName = "이름";
	private string slotScript = "설명";

	private void Start()
	{
		Init();
	}

	virtual public void Init()
	{
		Transform childTransformSlotImg = transform.Find("SlotImage");
		slotImage = childTransformSlotImg.GetComponent<Image>();

		Transform childTransformFrontImg = transform.Find("FrontImage");
		frontImage = childTransformFrontImg.GetComponent<Image>();
	}

	public virtual void SetInfo(string slotName, string slotScript)
    {
		this.slotName = slotName;
		this.slotScript = slotScript;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
		UITooltipParam uiTooltipParam = new UITooltipParam(slotName, slotScript);
		uiToolTip = Managers.UI.SpawnObjectUI<UI_ToolTip>(EUIObjectType.UI_Tooltip, uiTooltipParam);
	}

    public virtual void OnPointerExit(PointerEventData eventData)
    {
		if (uiToolTip != null)
			uiToolTip.DestroyUI();
		uiToolTip = null;
    }
}
