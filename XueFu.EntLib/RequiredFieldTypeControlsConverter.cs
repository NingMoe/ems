using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;

namespace XueFu.EntLib
{
    public class RequiredFieldTypeControlsConverter : StringConverter
    {
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            ArrayList values = new ArrayList();
            values.Add("����У��");
            values.Add("����У��");
            values.Add("��������");
            values.Add("�ƶ��ֻ�");
            values.Add("���õ绰");
            values.Add("���֤����");
            values.Add("��ҳ��ַ");
            values.Add("����");
            values.Add("����ʱ��");
            values.Add("���");
            values.Add("�ʱ�");
            values.Add("IP��ַ");
            values.Add("IP��ַ���˿�");
            values.Add("�Զ�����֤���ʽ");
            return new TypeConverter.StandardValuesCollection(values);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
