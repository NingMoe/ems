using System;
using System.Collections.Generic;
using System.Text;

namespace XueFu.EntLib
{
    public enum KPIType
    {
        [Enum("����ָ��")]
        Temp = 0,
        [Enum("����ָ��")]
        Fixed = 1
    }

    public enum DisplayTye
    {
        [Enum("ͼƬ")]
        Image = 2,
        [Enum("����")]
        Text = 1
    }

    public enum InputType
    {
        [Enum("��ѡ")]
        CheckBox = 6,
        [Enum("�ؼ���")]
        KeyWord = 3,
        [Enum("��ѡ")]
        Radio = 5,
        [Enum("����ѡ��")]
        Select = 4,
        [Enum("�����ı�")]
        Text = 1,
        [Enum("�����ı�")]
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
        [Enum("��Ʒ����")]
        Group = 2,
        [Enum("�޹��")]
        No = 0,
        [Enum("����Ʒ���")]
        Single = 1
    }

    public enum QuestionType
    {
        [Enum("����ѡ����")]
        Single = 1,
        [Enum("����ѡ����")]
        Multi = 2,
        [Enum("�ж���")]
        Panduan = 3
    }

    public enum ProductStorageType
    {
        ImportStorageSystem = 2,
        SelfStorageSystem = 1
    }

    public enum CompanyState
    {
        [Enum("<font color=red>����</font>")]
        NoCheck = 1,
        [Enum("����")]
        Normal = 0
    }

    public enum UserState
    {
        [Enum("ɾ��")]
        Del = 0,
        [Enum("δ��֤")]
        NoCheck = 1,
        [Enum("����")]
        Normal = 2,
        [Enum("����")]
        Frozen = 3,
        [Enum("������")]
        Free = 4,
        [Enum("����")]
        Other = 5
    }

    public enum State
    {
        [Enum("<font color=red>����</font>")]
        NoCheck = 1,
        [Enum("����")]
        Normal = 0
    }

    public enum SexType
    {
        [Enum("����")]
        Secret = 0,
        [Enum("��")]
        Male = 1,
        [Enum("Ů")]
        Female = 2
    }

}
