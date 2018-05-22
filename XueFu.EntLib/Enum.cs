using System;
using System.Collections.Generic;
using System.Text;

namespace XueFu.EntLib
{
    public enum KPIType
    {
        [Enum("当期指标")]
        Temp = 0,
        [Enum("永久指标")]
        Fixed = 1
    }

    public enum DisplayTye
    {
        [Enum("图片")]
        Image = 2,
        [Enum("文字")]
        Text = 1
    }

    public enum InputType
    {
        [Enum("多选")]
        CheckBox = 6,
        [Enum("关键字")]
        KeyWord = 3,
        [Enum("单选")]
        Radio = 5,
        [Enum("下拉选择")]
        Select = 4,
        [Enum("单行文本")]
        Text = 1,
        [Enum("多行文本")]
        Textarea = 2
    }

    public enum BoolType
    {
        False,
        True
    }

    public enum AdType
    {
        Code = 4,
        Flash = 3,
        Picture = 2,
        Text = 1
    }

    public enum DateType
    {
        Day = 1,
        Month = 2
    }

    public enum ProductStandardType
    {
        [Enum("产品组规格")]
        Group = 2,
        [Enum("无规格")]
        No = 0,
        [Enum("单产品规格")]
        Single = 1
    }

    public enum QuestionType
    {
        [Enum("单项选择题")]
        Single = 1,
        [Enum("多项选择题")]
        Multi = 2,
        [Enum("判断题")]
        Panduan = 3
    }

    public enum ProductStorageType
    {
        ImportStorageSystem = 2,
        SelfStorageSystem = 1
    }

    public enum CompanyState
    {
        [Enum("<font color=red>锁定</font>")]
        NoCheck = 1,
        [Enum("正常")]
        Normal = 0
    }

    public enum UserState
    {
        [Enum("删除")]
        Del = 0,
        [Enum("未验证")]
        NoCheck = 1,
        [Enum("正常")]
        Normal = 2,
        [Enum("冻结")]
        Frozen = 3,
        [Enum("不考核")]
        Free = 4,
        [Enum("其他")]
        Other = 5
    }

    public enum State
    {
        [Enum("<font color=red>锁定</font>")]
        NoCheck = 1,
        [Enum("正常")]
        Normal = 0
    }

    public enum SexType
    {
        [Enum("保密")]
        Secret = 0,
        [Enum("男")]
        Male = 1,
        [Enum("女")]
        Female = 2
    }

}
