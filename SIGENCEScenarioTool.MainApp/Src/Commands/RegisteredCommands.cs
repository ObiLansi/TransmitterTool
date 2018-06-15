﻿using System.Windows.Input;



namespace SIGENCEScenarioTool.Commands
{
    /// <summary>
    /// 
    /// </summary>
    static public class RegisteredCommands
    {
        /// <summary>
        /// Gets the open cheat sheet.
        /// </summary>
        /// <value>
        /// The open cheat sheet.
        /// </value>
        static public RoutedUICommand OpenCheatSheet { get; private set; }

        //---------------------------------------------------------------------

        /// <summary>
        /// Gets the create RFDevice.
        /// </summary>
        /// <value>
        /// The create RFDevice.
        /// </value>
        static public RoutedUICommand CreateRFDevice { get; private set; }

        /// <summary>
        /// Gets the delete RFDevice.
        /// </summary>
        /// <value>
        /// The delete RFDevice.
        /// </value>
        static public RoutedUICommand DeleteRFDevice { get; private set; }


        /// <summary>
        /// Gets the copy rf device.
        /// </summary>
        /// <value>
        /// The copy rf device.
        /// </value>
        static public RoutedUICommand CopyRFDevice { get; private set; }

        /// <summary>
        /// Gets the paste rf device.
        /// </summary>
        /// <value>
        /// The paste rf device.
        /// </value>
        static public RoutedUICommand PasteRFDevice { get; private set; }

        /// <summary>
        /// Gets the export RFDevice.
        /// </summary>
        /// <value>
        /// The export RFDevice.
        /// </value>
        static public RoutedUICommand ExportRFDevice { get; private set; }


        /// <summary>
        /// Gets the import RFDevice.
        /// </summary>
        /// <value>
        /// The import RFDevice.
        /// </value>
        static public RoutedUICommand ImportRFDevice { get; private set; }

        //---------------------------------------------------------------------

        /// <summary>
        /// Gets the create screenshot.
        /// </summary>
        /// <value>
        /// The create screenshot.
        /// </value>
        static public RoutedUICommand CreateScreenshot { get; private set; }


        /// <summary>
        /// Gets the send data UDP.
        /// </summary>
        /// <value>
        /// The send data UDP.
        /// </value>
        static public RoutedUICommand SendDataUDP { get; private set; }

        /// <summary>
        /// Gets the zoom to rf device.
        /// </summary>
        /// <value>
        /// The zoom to rf device.
        /// </value>
        static public RoutedUICommand ZoomToRFDevice { get; private set; }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Initializes the <see cref="RegisteredCommands"/> class.
        /// </summary>
        static RegisteredCommands()
        {
            OpenCheatSheet = new RoutedUICommand( "OpenCheatSheet" , "OpenCheatSheet" , typeof( RegisteredCommands ) );
            OpenCheatSheet.InputGestures.Add( new KeyGesture( Key.F1 ) );

            CreateRFDevice = new RoutedUICommand( "CreateRFDevice" , "CreateRFDevice" , typeof( RegisteredCommands ) );
            CreateRFDevice.InputGestures.Add( new KeyGesture( Key.F5 ) );
            CreateRFDevice.InputGestures.Add( new KeyGesture( Key.C , ModifierKeys.Alt ) );

            DeleteRFDevice = new RoutedUICommand( "DeleteRFDevice" , "DeleteRFDevice" , typeof( RegisteredCommands ) );
            DeleteRFDevice.InputGestures.Add( new KeyGesture( Key.F6 ) );
            DeleteRFDevice.InputGestures.Add( new KeyGesture( Key.D , ModifierKeys.Alt ) );

            CopyRFDevice = new RoutedUICommand( "CopyRFDevice" , "CopyRFDevice" , typeof( RegisteredCommands ) );
            //CopyRFDevice.InputGestures.Add( new KeyGesture( Key.C , ModifierKeys.Control ) );

            PasteRFDevice = new RoutedUICommand( "PasteRFDevice" , "PasteRFDevice" , typeof( RegisteredCommands ) );
            //PasteRFDevice.InputGestures.Add( new KeyGesture( Key.V , ModifierKeys.Control ) );

            ExportRFDevice = new RoutedUICommand( "ExportRFDevice" , "ExportRFDevice" , typeof( RegisteredCommands ) );
            ExportRFDevice.InputGestures.Add( new KeyGesture( Key.F7 ) );
            ExportRFDevice.InputGestures.Add( new KeyGesture( Key.E , ModifierKeys.Alt ) );

            ImportRFDevice = new RoutedUICommand( "ImportRFDevice" , "ImportRFDevice" , typeof( RegisteredCommands ) );
            ImportRFDevice.InputGestures.Add( new KeyGesture( Key.F8 ) );
            ImportRFDevice.InputGestures.Add( new KeyGesture( Key.I , ModifierKeys.Alt ) );


            ZoomToRFDevice = new RoutedUICommand( "ZoomToRFDevice" , "ZoomToRFDevice" , typeof( RegisteredCommands ) );
            ZoomToRFDevice.InputGestures.Add( new KeyGesture( Key.F9 ) );
            ZoomToRFDevice.InputGestures.Add( new KeyGesture( Key.Z , ModifierKeys.Control ) );

            CreateScreenshot = new RoutedUICommand( "CreateScreenshot" , "CreateScreenshot" , typeof( RegisteredCommands ) );
            CreateScreenshot.InputGestures.Add( new KeyGesture( Key.F10 ) );
            CreateScreenshot.InputGestures.Add( new KeyGesture( Key.T , ModifierKeys.Control ) );

            // F11 is reserved for fullscreen ...

            SendDataUDP = new RoutedUICommand( "SendDataUDP" , "SendDataUDP" , typeof( RegisteredCommands ) );
            SendDataUDP.InputGestures.Add( new KeyGesture( Key.F12 ) );
            SendDataUDP.InputGestures.Add( new KeyGesture( Key.U , ModifierKeys.Control ) );
        }

    } // end static public class RegisteredCommands
}
