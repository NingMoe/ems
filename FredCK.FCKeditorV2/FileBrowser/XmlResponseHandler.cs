using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Web;

namespace FredCK.FCKeditorV2.FileBrowser
{
    internal class XmlResponseHandler
    {
        private Connector _Connector;
        private HttpResponse _Response;
        private XmlDocument _Xml;

        internal XmlResponseHandler(Connector connector, HttpResponse response)
        {
            this._Connector = connector;
            this._Response = response;
        }

        public XmlNode CreateBaseXml(string command, string resourceType, string currentFolder)
        {
            this.Xml.AppendChild(this.Xml.CreateXmlDeclaration("1.0", "utf-8", null));
            XmlNode node = XmlUtil.AppendElement(this.Xml, "Connector");
            XmlUtil.SetAttribute(node, "command", command);
            XmlUtil.SetAttribute(node, "resourceType", resourceType);
            XmlNode node2 = XmlUtil.AppendElement(node, "CurrentFolder");
            XmlUtil.SetAttribute(node2, "path", currentFolder);
            XmlUtil.SetAttribute(node2, "url", this.Connector.GetUrlFromPath(resourceType, currentFolder));
            return node;
        }

        internal static void SendError(HttpResponse response, int errorNumber, string errorText)
        {
            SetupResponse(response);
            response.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            response.Write("<Connector>");
            response.Write(string.Concat(new object[] { "<Error number=\"", errorNumber, "\" text=\"", HttpUtility.HtmlEncode(errorText), "\" />" }));
            response.Write("</Connector>");
            response.End();
        }

        public void SendResponse()
        {
            this.SetupResponse();
            this.Response.Write(this.Xml.OuterXml);
            this.Response.End();
        }

        private void SetupResponse()
        {
            SetupResponse(this.Response);
        }

        private static void SetupResponse(HttpResponse response)
        {
            response.ClearHeaders();
            response.Clear();
            response.CacheControl = "no-cache";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = "text/xml";
        }

        // Properties
        private Connector Connector
        {
            get
            {
                return this._Connector;
            }
        }

        private HttpResponse Response
        {
            get
            {
                return this._Response;
            }
        }

        private XmlDocument Xml
        {
            get
            {
                if (this._Xml == null)
                {
                    this._Xml = new XmlDocument();
                }
                return this._Xml;
            }
        }
    }
}
