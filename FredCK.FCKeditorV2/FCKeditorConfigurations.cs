using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections;

namespace FredCK.FCKeditorV2
{
    [Serializable]
    public class FCKeditorConfigurations : ISerializable
    {
        // Fields
        private Hashtable _Configs;

        // Methods
        internal FCKeditorConfigurations()
        {
            this._Configs = new Hashtable();
        }

        protected FCKeditorConfigurations(SerializationInfo info, StreamingContext context)
        {
            this._Configs = (Hashtable)info.GetValue("ConfigTable", typeof(Hashtable));
        }

        private string EncodeConfig(string valueToEncode)
        {
            return valueToEncode.Replace("&", "%26").Replace("=", "%3D").Replace("\"", "%22");
        }

        internal string GetHiddenFieldString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (DictionaryEntry entry in this._Configs)
            {
                if (builder.Length > 0)
                {
                    builder.Append("&amp;");
                }
                builder.AppendFormat("{0}={1}", this.EncodeConfig(entry.Key.ToString()), this.EncodeConfig(entry.Value.ToString()));
            }
            if (!this._Configs.Contains("HtmlEncodeOutput"))
            {
                if (builder.Length > 0)
                {
                    builder.Append("&amp;");
                }
                builder.Append("HtmlEncodeOutput=true");
            }
            return builder.ToString();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ConfigTable", this._Configs);
        }

        // Properties
        public string this[string configurationName]
        {
            get
            {
                if (this._Configs.ContainsKey(configurationName))
                {
                    return (string)this._Configs[configurationName];
                }
                return null;
            }
            set
            {
                this._Configs[configurationName] = value;
            }
        }
    }
}
