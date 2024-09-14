using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ToolTip : UI_BaseObject
{
    [SerializeField, ReadOnly] public TextMeshProUGUI scriptText;
    [SerializeField, ReadOnly] public TextMeshProUGUI nameText;

    [SerializeField, ReadOnly] public RectTransform rt;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvas(gameObject, false, 10);

        Vector3 pos = Input.mousePosition;
        pos.x += rt.rect.width / 2 + 10;
        pos.y += rt.rect.height / 2 + 10;
        rt.position = pos;

        rt.SetAsFirstSibling();

        return true;
    }

    private void Update()
    {
        Vector3 pos = Input.mousePosition;
        pos.x += rt.rect.width / 2 + 10;
        pos.y += rt.rect.height / 2 + 10;
        rt.position = pos;
    }

    public override void SetInfo(UIParam param)
    {
        base.SetInfo(param);

        UITooltipParam test = param as UITooltipParam;

        if (test == null)
            return;

        if (test is UITooltipParam uiTooltipParam)
        {
            nameText.text = uiTooltipParam.nameText;
            scriptText.text = uiTooltipParam.scriptText;
        }
    }

    public void DestroyUI()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
