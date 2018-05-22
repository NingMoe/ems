using System;
using System.Collections.Generic;
using System.Text;

namespace FredCK.FCKeditorV2.FileBrowser
{
    public class Uploader : FileWorkerBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.Config.LoadConfig();
            if (!base.Config.Enabled)
            {
                base.SendFileUploadResponse(1, true, "", "", "This connector is disabled. Please check the \"editor/filemanager/connectors/aspx/config.aspx\" file.");
            }
            else
            {
                string typeName = base.Request.QueryString["Type"];
                if (typeName == null)
                {
                    base.SendFileUploadResponse(1, true, "", "", "Invalid request.");
                }
                else if (!base.Config.CheckIsTypeAllowed(typeName))
                {
                    base.SendFileUploadResponse(1, true, "", "", "Invalid resource type specified.");
                }
                else
                {
                    base.FileUpload(typeName, "/", true);
                }
            }
        }
    }
}
