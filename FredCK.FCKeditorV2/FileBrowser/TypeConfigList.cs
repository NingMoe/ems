using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace FredCK.FCKeditorV2.FileBrowser
{
    public class TypeConfigList
    {
        private FileWorkerBase _FileWorker;
        private Hashtable _Types;

        public TypeConfigList(FileWorkerBase fileWorker)
        {
            this._FileWorker = fileWorker;
            this._Types = new Hashtable(4);
        }

        public TypeConfig this[string typeName]
        {
            get
            {
                if (!this._Types.Contains(typeName))
                {
                    this._Types[typeName] = new TypeConfig(this._FileWorker);
                }
                return (TypeConfig)this._Types[typeName];
            }
        }
    }
}
