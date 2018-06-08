﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Input;
using System.Xml.Linq;

using GMap.NET;

using SIGENCEScenarioTool.Datatypes.Standard;
using SIGENCEScenarioTool.Extensions;
using SIGENCEScenarioTool.Models;
using SIGENCEScenarioTool.Tools;
using SIGENCEScenarioTool.ViewModels;

using Excel = global::Microsoft.Office.Interop.Excel;



namespace SIGENCEScenarioTool.Windows.MainWindow
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainWindow
    {

        /// <summary>
        /// Gets the device list.
        /// </summary>
        /// <returns>A RFDeviceList with all selected RFDevices from the DataGrid.</returns>
        private RFDeviceList GetDeviceList()
        {
            return new RFDeviceList( from devicemodel in RFDevicesCollection where devicemodel.IsSelected == true select devicemodel.RFDevice );
        }


        /// <summary>
        /// Sets the map to creating RFDevice mode.
        /// </summary>
        private void SetMapToCreatingRFDeviceMode()
        {
            mcMapControl.DragButton = bCreatingRFDevice ? MouseButton.Right : MouseButton.Left;
        }


        /// <summary>
        /// Begins the create RFDevice.
        /// </summary>
        private void BeginCreateRFDevice()
        {
            CreatingRFDevice = true;
            mcMapControl.Cursor = Cursors.Cross;
        }


        /// <summary>
        /// Ends the create RFDevice.
        /// </summary>
        private void EndCreateRFDevice()
        {
            mcMapControl.Cursor = Cursors.Arrow;
            CreatingRFDevice = false;
        }


        /// <summary>
        /// Creates the RFDevice.
        /// </summary>
        /// <param name="pll">The PLL.</param>
        private void AddRFDevice( PointLatLng pll )
        {
            AddRFDevice( new RFDevice
            {
                Latitude = pll.Lat ,
                Longitude = pll.Lng
            } );
        }


        /// <summary>
        /// Adds the RFDevice.
        /// </summary>
        /// <param name="t">The t.</param>
        private void AddRFDevice( RFDevice t )
        {
            RFDeviceViewModel tvm = new RFDeviceViewModel( t );

            RFDevicesCollection.Add( tvm );
            mcMapControl.Markers.Add( tvm.Marker );
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Deletes the RFDevice.
        /// </summary>
        /// <param name="tvm">The TVM.</param>
        private void DeleteRFDevice( RFDeviceViewModel tvm )
        {
            RFDevicesCollection.Remove( tvm );
            mcMapControl.Markers.Remove( tvm.Marker );
        }


        /// <summary>
        /// Deletes the RFDevice.
        /// </summary>
        private void DeleteRFDevice()
        {
            if( dgRFDevices.SelectedItem == null )
            {
                MB.Information( "No RFDevice Is Selected In The DataGrid!" );
                return;
            }

            if( dgRFDevices.SelectedItems.Count > 1 )
            {
                MB.Information( "There Are More Than One RFDevice Selected In The DataGrid!" );
                return;
            }

            DeleteRFDevice( dgRFDevices.SelectedItem as RFDeviceViewModel );
        }


        /// <summary>
        /// Deletes the RFDevices.
        /// </summary>
        private void DeleteRFDevices()
        {
            foreach( RFDeviceViewModel device in ( from devicemodel in RFDevicesCollection where devicemodel.IsSelected == true select devicemodel ).ToList() )
            {
                DeleteRFDevice( device );
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        #region Specialied Excel Export

        /// <summary>
        /// Adds the cell.
        /// </summary>
        /// <param name="sheet">The sheet.</param>
        /// <param name="iColumn">The i column.</param>
        /// <param name="iRow">The i row.</param>
        /// <param name="value">The value.</param>
        private void AddCell( Excel.Worksheet sheet , int iColumn , int iRow , object value )
        {
            Excel.Range cell = sheet.Cells [iRow , iColumn] as Excel.Range;
            cell.Value2 = value;
            cell.HorizontalAlignment = value is string ? Excel.XlHAlign.xlHAlignLeft : Excel.XlHAlign.xlHAlignRight;
        }


        /// <summary>
        /// Saves as excel file.
        /// </summary>
        /// <param name="dl">The dl.</param>
        /// <param name="strOutputFilename">The string output filename.</param>
        /// <exception cref="ArgumentException">Der Ausgabedateiname darf nicht leer sein! - strOutputFilename</exception>
        private void SaveAsExcel( RFDeviceList dl , string strOutputFilename )
        {
            if( strOutputFilename.IsEmpty() )
            {
                throw new ArgumentException( "The output filename can not be empty!" , "strOutputFilename" );
            }

            //-----------------------------------------------------------------

            Excel.Application excel = new Excel.Application
            {
                SheetsInNewWorkbook = 1
            };

            Excel.Workbook wb = excel.Workbooks.Add( Missing );
            Excel.Worksheet sheet = wb.Sheets [1] as Excel.Worksheet;

            sheet.Name = "RF Devices";

            //-----------------------------------------------------------------

            StringList slColumnNames = new StringList
            {
                RFDevice.STARTTIME,RFDevice.ID,
                RFDevice.LATITUDE,RFDevice.LONGITUDE,RFDevice.ALTITUDE,
                RFDevice.ROLL,RFDevice.PITCH,RFDevice.YAW ,
                RFDevice.RXTXTYPE,RFDevice.ANTENNATYPE,
                RFDevice.GAIN_DB,RFDevice.CENTERFREQUENCY_HZ,RFDevice.BANDWITH_HZ,RFDevice.SIGNALTONOISERATIO_DB,
                RFDevice.XPOS,RFDevice.YPOS,RFDevice.ZPOS,
                RFDevice.REMARK
            };

            // Create Header Columns
            {
                int iColumnCounter = 1;

                foreach( string strColumn in slColumnNames )
                {
                    Excel.Range cell = sheet.Cells [1 , iColumnCounter++] as Excel.Range;
                    cell.Font.Bold = true;
                    cell.Orientation = Excel.XlOrientation.xlUpward;
                    cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    cell.VerticalAlignment = Excel.XlVAlign.xlVAlignBottom;
                    cell.Value2 = " " + strColumn;
                }
            }

            //-----------------------------------------------------------------

            // Create Data Columns And Rows
            {
                int iRowCounter = 2;

                foreach( RFDevice device in dl )
                {
                    AddCell( sheet , 1 , iRowCounter , device.StartTime );
                    AddCell( sheet , 2 , iRowCounter , device.Id );
                    AddCell( sheet , 3 , iRowCounter , device.Latitude );
                    AddCell( sheet , 4 , iRowCounter , device.Longitude );
                    AddCell( sheet , 5 , iRowCounter , device.Altitude );
                    AddCell( sheet , 6 , iRowCounter , device.Roll );
                    AddCell( sheet , 7 , iRowCounter , device.Pitch );
                    AddCell( sheet , 8 , iRowCounter , device.Yaw );
                    AddCell( sheet , 9 , iRowCounter , device.RxTxType );
                    AddCell( sheet , 10 , iRowCounter , device.AntennaType );
                    AddCell( sheet , 11 , iRowCounter , device.Gain_dB );
                    AddCell( sheet , 12 , iRowCounter , device.CenterFrequency_Hz );
                    AddCell( sheet , 13 , iRowCounter , device.Bandwith_Hz );
                    AddCell( sheet , 14 , iRowCounter , device.SignalToNoiseRatio_dB );
                    AddCell( sheet , 15 , iRowCounter , device.XPos );
                    AddCell( sheet , 16 , iRowCounter , device.YPos );
                    AddCell( sheet , 17 , iRowCounter , device.ZPos );
                    AddCell( sheet , 18 , iRowCounter , device.Remark );

                    iRowCounter++;
                }
            }

            //-----------------------------------------------------------------

            sheet.Columns.AutoFit();

            excel.Visible = true;

            wb.SaveAs( strOutputFilename , Missing , Missing , Missing , Missing , Missing , Excel.XlSaveAsAccessMode.xlNoChange , Missing , Missing , Missing , Missing , Missing );

            // Achtung: Auch wenn diese Funktion beendet wird bleibt Excel geöffnet. Die Daten sind
            // aber noch nicht in einer Datei gespeichert. Das muß in Excel der User selbst machen.
        }

        #endregion

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Exports the rf devices.
        /// </summary>
        /// <param name="devicelist">The devicelist.</param>
        /// <param name="fiExportFile">The fi export file.</param>
        private void ExportRFDevices( RFDeviceList devicelist , FileInfo fiExportFile )
        {
            Cursor = Cursors.Wait;

            DoEvents();

            try
            {
                switch( fiExportFile.Extension.ToLower() )
                {
                    case ".csv":
                        devicelist.SaveAsCsv( fiExportFile.FullName );
                        MB.Information( "File {0} successful created." , fiExportFile.Name );
                        break;

                    case ".json":
                        devicelist.SaveAsJson( fiExportFile.FullName );
                        MB.Information( "File {0} successful created." , fiExportFile.Name );
                        break;

                    case ".xml":
                        devicelist.SaveAsXml( fiExportFile.FullName );
                        MB.Information( "File {0} successful created." , fiExportFile.Name );
                        break;

                    case ".xlsx":
                        SaveAsExcel( devicelist , fiExportFile.FullName );
                        break;
                }
            }
            catch( Exception ex )
            {
                MB.Error( ex );
            }

            Cursor = Cursors.Arrow;
        }


        /// <summary>
        /// Exports the RFDevices.
        /// </summary>
        private void ExportRFDevices()
        {
            RFDeviceList devicelist = GetDeviceList();

            if( devicelist.Count == 0 )
            {
                MB.Warning( "No Selected RFDevice Avaible For Export!" );
                return;
            }

            sfdExportRFDevices.FileName = CurrentFile != null ? new FileInfo( CurrentFile ).Name : DateTime.Now.Fmt_YYYYMMDDHHMMSS();

            if( sfdExportRFDevices.ShowDialog() == true )
            {
                ExportRFDevices( devicelist , new FileInfo( sfdExportRFDevices.FileName ) );
            }
        }


        /// <summary>
        /// Imports the RFDevices.
        /// </summary>
        private void ImportRFDevices()
        {
            MB.NotYetImplemented();
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Zooms to rf device.
        /// </summary>
        /// <param name="device">The device.</param>
        private void ZoomToRFDevice( RFDevice device )
        {
            mcMapControl.Position = new PointLatLng( device.Latitude , device.Longitude );
            mcMapControl.Zoom = settings.MapZoomLevel;
        }


        /// <summary>
        /// Zooms to rf device.
        /// </summary>
        private void ZoomToRFDevice()
        {
            if( dgRFDevices.SelectedItem == null )
            {
                MB.Information( "No RFDevice Is Selected In The DataGrid!" );
                return;
            }

            if( dgRFDevices.SelectedItems.Count > 1 )
            {
                MB.Information( "There Are More Than One RFDevice Selected In The DataGrid!" );
                return;
            }

            ZoomToRFDevice( ( dgRFDevices.SelectedItem as RFDeviceViewModel ).RFDevice );
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Sends the RFDeviceList via UDP to any connect client.
        /// This function is not asynchron, so the main thread is blocked when sending data. 
        /// Maybe in oen of the next versions me make this function asynchron.
        /// </summary>
        /// <param name="devicelist">The devicelist.</param>
        private void SendDataUDP( RFDeviceList devicelist )
        {
            //Task.Run( () =>
            //     Speech.Say( "Starting transfer of radio frequency devices" )
            //);

            try
            {
                using( Socket sender = new Socket( AddressFamily.InterNetwork , SocketType.Dgram , ProtocolType.Udp ) )
                {
                    IPEndPoint endpoint = new IPEndPoint( IPAddress.Parse( settings.UDPHost ) , settings.UDPPort );

                    foreach( RFDevice device in devicelist )
                    {
                        // Settings the StartTime relativ to the starttime of the tool in milliseconds ...
                        device.StartTime = ( DateTime.Now - dtStartTime ).TotalMilliseconds;
                        
                        XElement eDevice = device.ToXml();

                        byte [] baMessage = Encoding.Default.GetBytes( eDevice.ToDefaultString() );

                        sender.SendTo( baMessage , endpoint );

                        // Give the poor client some time to process the data when he need or bleed ...
                        if( settings.UDPDelay > 0 )
                        {
                            Thread.Sleep( settings.UDPDelay );
                        }
                    }

                    sender.Close();
                }
            }
            catch( Exception ex )
            {
                MB.Error( ex );
            }

            //Task.Run( () =>
            //    Speech.Say( "Finished with sending the data" )
            //);
        }


        /// <summary>
        /// Sends the data UDP.
        /// </summary>
        private void SendDataUDP()
        {
            RFDeviceList devicelist = GetDeviceList();

            if( devicelist.Count == 0 )
            {
                MB.Warning( "No Selected RFDevice Avaible For Sending!" );
                return;
            }

            SendDataUDP( devicelist );
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// The randomizer.
        /// </summary>
        static private readonly Random r = new Random();


        /// <summary>
        /// Creates the randomized RFDevices.
        /// </summary>
        /// <param name="iMaxCount">The i maximum count.</param>
        private void CreateRandomizedRFDevices( int iMaxCount )
        {
            Cursor = Cursors.Wait;
            DoEvents();

            for( int iCounter = 1 ; iCounter < iMaxCount + 1 ; iCounter++ )
            {
                RFDevice t = new RFDevice
                {
                    Id = r.Next( -1000 , 1000 ) ,
                    Name = string.Format( "RFDevice #{0}" , iCounter ) ,
                    Latitude = ( r.NextDouble() * 0.05 ) + 49.7454 ,
                    Longitude = ( r.NextDouble() * 0.05 ) + 6.6149 ,
                    Altitude = 0 ,
                    RxTxType = r.NextEnum<RxTxType>() ,
                    AntennaType = r.NextEnum<AntennaType>() ,
                    CenterFrequency_Hz = ( uint ) r.Next( 85 , 105 ) * 100000 ,
                    Bandwith_Hz = ( uint ) r.Next( 10 , 20 ) * 1000 ,
                    Gain_dB = 0 ,
                    SignalToNoiseRatio_dB = 0 ,
                    Roll = 0 ,
                    Pitch = 0 ,
                    Yaw = 0 ,
                    XPos = 0 ,
                    YPos = 0 ,
                    ZPos = 0 ,
                    Remark = r.NextObject( Tool.ALLPANGRAMS )
                };

                AddRFDevice( t );
            }

            Cursor = Cursors.Arrow;
        }

    } // end public partial class MainWindow
}
