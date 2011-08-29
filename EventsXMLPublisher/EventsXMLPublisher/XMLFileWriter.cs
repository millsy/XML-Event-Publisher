using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Design;

using OpenSpan.ComponentModel;
using OpenSpan.Design;
using OpenSpan.Diagnostics;
using OpenSpan.TypeManagement;
using System.ComponentModel;
using System.IO;
using OpenSpan.Events;

namespace EventsXMLPublisher
{
    [ToolboxBitmap(typeof(XMLFileWriter), "EventConnector.ico")]
    [ObjectExplorerGroupMember(ObjectExplorerGroupName.Events)]
    public partial class XMLFileWriter : BaseConnector
    {
        public XMLFileWriter()
        {

        }

        private string mFilePath;
        private bool mLoggingEnabled;

        [RefreshProperties(RefreshProperties.All)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [MemberVisibility(MemberVisibilityLevel.DefaultOn)]
        public string FilePath
        {
            get { return mFilePath; }
            set
            {
                if (value == null || !Directory.Exists(value))
                    value = string.Empty;

                mFilePath = value;
            }
        }

        [Browsable(true)]
        [MemberVisibility(MemberVisibilityLevel.DefaultOn)]
        [Description("Enables/disables the logging of events to the designated dir")]
        public bool LoggingEnabled
        {
            get { return mLoggingEnabled; }
            set { mLoggingEnabled = value; }
        }

        protected override void ProcessEvent(EventOccurredArgs args)
        {
            // if logging is disabled, don't write this out...
            if (mLoggingEnabled == false) return;

            if (mFilePath != string.Empty)
            {
                LogEvent(args, mFilePath);
            }
        }

        private void LogEvent(EventOccurredArgs args, string fileName)
        {
            try
            {
                string eventStr = EventOccurredArgs.Serialize(args);
                Guid guid = Guid.NewGuid();

                File.WriteAllText(Path.Combine(fileName, guid.ToString() + ".xml"), eventStr);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
