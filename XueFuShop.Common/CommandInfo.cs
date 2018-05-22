using System;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;


public enum PowerCheckType
{
    Single,
    OR,
    AND
}

public enum DataState
{
    [Enum("����")]
    Normal = 0,
    [Enum("ɾ��")]
    Delete = 1,
    [Enum("<font color=red>����</font>")]
    Lock = 2
}

public enum ChangeAction
{
    Up,
    Down,
    Plus,
    Minus
}

public enum CompanyType
{
    [Enum("��˾")]
    Company,
    [Enum("����")]
    Group,
    [Enum("�Ӽ���")]
    SubGroup,
    [Enum("�ӹ�˾")]
    SubCompany
}

//public enum SexType
//{
//    [Enum("��")]
//    Men = 1,
//    [Enum("����")]
//    Secret = 3,
//    [Enum("Ů")]
//    Women = 2
//}

public enum DateType
{
    Day = 1,
    Month = 2
}

public enum ShippingType
{
    Fixed = 1,
    ProductCount = 3,
    Weight = 2
}

public enum OrderStatus
{
    [Enum("���˻�")]
    HasReturn = 7,
    [Enum("�ѷ���")]
    HasShipping = 5,
    [Enum("��Ч")]
    NoEffect = 3,
    [Enum("���ջ�")]
    ReceiveShipping = 6,
    [Enum("�����")]
    Shipping = 4,
    [Enum("�����")]
    WaitCheck = 2,
    [Enum("������")]
    WaitPay = 1
}

public enum SendStatus
{
    [Enum("�������")]
    Finished = 3,
    [Enum("δ����")]
    No = 1,
    [Enum("������")]
    Sending = 2
}

 


public class EnumAttribute : Attribute
{
    private string chineseName;

    public EnumAttribute(string chineseName)
    {
        this.chineseName = chineseName;
    }

    public string ChineseName
    {
        get{return this.chineseName;}
        set{this.chineseName = value;}
    }
}

[Serializable, Flags, ComVisible(true)]
public enum NumberStyles
{
    AllowCurrencySymbol = 0x100,
    AllowDecimalPoint = 0x20,
    AllowExponent = 0x80,
    AllowHexSpecifier = 0x200,
    AllowLeadingSign = 4,
    AllowLeadingWhite = 1,
    AllowParentheses = 0x10,
    AllowThousands = 0x40,
    AllowTrailingSign = 8,
    AllowTrailingWhite = 2,
    Any = 0x1ff,
    Currency = 0x17f,
    Float = 0xa7,
    HexNumber = 0x203,
    Integer = 7,
    None = 0,
    Number = 0x6f
}

public sealed class GetList
{
    public static string GetListValue(CheckBoxList BoxName)
    {
        string Temp = string.Empty;
        foreach (ListItem Item in BoxName.Items)
        {
            if (Item.Selected)
            {
                Temp = Temp + "," + Item.Value;
            }
        }
        if (Temp != string.Empty) return Temp.Substring(1);
        else return string.Empty;
    }

    public static string GetListText(CheckBoxList BoxName)
    {
        string Temp = string.Empty;
        foreach (ListItem Item in BoxName.Items)
        {
            if (Item.Selected)
            {
                Temp = Temp + "," + Item.Text;
            }
        }
        if (Temp != string.Empty) return Temp.Substring(1);
        else return string.Empty;
    }

    public static void SetListCheckedByValue(CheckBoxList BoxName, string Value)
    {
        string Temp = string.Empty;
        string[] ItemValue = Value.Split(',');
        BoxName.ClearSelection();
        foreach (string Item in ItemValue)
        {
            if (BoxName.Items.Contains(BoxName.Items.FindByValue(Item)))
            {
                BoxName.Items.FindByValue(Item).Selected = true;
            }
        }
    }

    public static string GetListValue(RadioButtonList BoxName)
    {
        string Temp = string.Empty;
        foreach (ListItem Item in BoxName.Items)
        {
            if (Item.Selected)
            {
                Temp = Temp + "," + Item.Value;
            }
        }
        if (Temp != string.Empty) return Temp.Substring(1);
        else return string.Empty;
    }

    public static string GetListText(RadioButtonList BoxName)
    {
        string Temp = string.Empty;
        foreach (ListItem Item in BoxName.Items)
        {
            if (Item.Selected)
            {
                Temp = Temp + "," + Item.Text;
            }
        }
        if (Temp != string.Empty) return Temp.Substring(1);
        else return string.Empty;
    }
}




