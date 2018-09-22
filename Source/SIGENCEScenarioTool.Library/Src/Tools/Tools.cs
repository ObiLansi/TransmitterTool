using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;



namespace SIGENCEScenarioTool.Tools
{
    internal struct SSTPoint
    {
        private readonly int x;
        private readonly int y;

        public new string ToString => string.Format($"{ this.x }/{ this.y }");
    }

    public struct SSTTniop
    {
        public int x;
        public int y;

        public SSTTniop(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


    /// <summary>
    /// Klasse mit statischen Standalonefunktionen.
    /// </summary>
    public static class Tool
    {
        /// <summary>
        /// Franz jagt im komplett verwahrlosten Taxi quer durch Bayern.
        /// </summary>
        public static readonly string FRANZ = "Franz jagt im komplett verwahrlosten Taxi quer durch Bayern.";

        /// <summary>
        /// The quick brown fox jumps over a lazy dog.
        /// </summary>
        public static readonly string FOX = "The quick brown fox jumps over a lazy dog.";

        /// <summary>
        /// Vom Ödipuskomplex maßlos gequält, übt Wilfried zyklisches Jodeln.
        /// </summary>
        public static readonly string WILFRIED = "Vom Ödipuskomplex maßlos gequält, übt Wilfried zyklisches Jodeln.";

        /// <summary>
        /// Falsches Üben von Xylophonmusik quält jeden größeren Zwerg.
        /// </summary>
        public static readonly string XYLOPHONMUSIK = "Falsches Üben von Xylophonmusik quält jeden größeren Zwerg.";

        /// <summary>
        /// The allchars
        /// </summary>
        public static readonly string ALLCHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZÖÄÜabcdefghijklmnopqrstuvwxyzöäü0123456789/*-+!\"§$%&()=?,.;:@€µ#'´`<>|^°\\";

        /// <summary>
        /// The allpangrams
        /// </summary>
        public static readonly List<string> ALLPANGRAMS = new List<string> { FRANZ, FOX, WILFRIED, XYLOPHONMUSIK };

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public static string ProductName { get; private set; }

        /// <summary>
        /// Gets the product title.
        /// </summary>
        /// <value>
        /// The product title.
        /// </value>
        public static string ProductTitle { get; private set; }

        /// <summary>
        /// Gets the startup path.
        /// </summary>
        /// <value>
        /// The startup path.
        /// </value>
        public static string StartupPath { get; private set; }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public static string Version { get; private set; }

        /// <summary>
        /// Gets the name of the module file.
        /// </summary>
        /// <param name="hModule">The h module.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Initializes the <see cref="Tools" /> class.
        /// </summary>
        static Tool()
        {
            StringBuilder buffer = new StringBuilder(256);
            GetModuleFileName(new HandleRef(null, IntPtr.Zero), buffer, buffer.Capacity);
            StartupPath = Path.GetDirectoryName(buffer.ToString());

            try
            {
                Assembly aEntryAssembly = Assembly.GetEntryAssembly();

                if (aEntryAssembly != null)
                {
                    {
                        object[] oAttributes = aEntryAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);

                        if (oAttributes.Length == 1)
                        {
                            ProductName = (oAttributes[0] as AssemblyProductAttribute).Product;
                        }
                    }
                    {
                        object[] oAttributes = aEntryAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

                        if (oAttributes.Length == 1)
                        {
                            ProductTitle = (oAttributes[0] as AssemblyTitleAttribute).Title;
                        }
                    }
                    {
                        object[] oAttributes = aEntryAssembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);

                        if (oAttributes.Length == 1)
                        {
                            Version = (oAttributes[0] as AssemblyFileVersionAttribute).Version;
                        }
                    }
                }
                else
                {
                    ProductName = "Unknown";
                    ProductTitle = "Unknown";
                    Version = "0.0";
                }
            }
            catch (Exception ex)
            {
                ProductName = ex.Message;
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Gets the size of the human.
        /// </summary>
        /// <param name="lSizeInBytes">The l size in bytes.</param>
        /// <returns></returns>
        public static string GetHumanSize(long lSizeInBytes)
        {
            if (lSizeInBytes >= 1099511627776)
            {
                return string.Format("{0:F} Tb", (float)lSizeInBytes / 1099511627776);
            }

            if (lSizeInBytes >= 1073741824)
            {
                return string.Format("{0:F} Gb", (float)lSizeInBytes / 1073741824);
            }

            if (lSizeInBytes >= 1048576)
            {
                return string.Format("{0:F} Mb", (float)lSizeInBytes / 1048576);
            }

            if (lSizeInBytes >= 1024)
            {
                return string.Format("{0:F} Kb", (float)lSizeInBytes / 1024);
            }

            return string.Format("{0} Bytes", lSizeInBytes);
        }


        /// <summary>
        /// Gets the human distance.
        /// </summary>
        /// <param name="lLengthInMeter">The l length in meter.</param>
        /// <returns></returns>
        public static string GetHumanDistance(long lLengthInMeter)
        {
            if (lLengthInMeter < 1000)
            {
                return string.Format("{0} m", lLengthInMeter);
            }

            return string.Format("{0:F} km", (float)lLengthInMeter / 1000);
        }


        /// <summary>
        /// Reads the resource as string.
        /// </summary>
        /// <param name="strResourceName">Name of the string resource.</param>
        /// <returns></returns>
        public static string ReadResourceAsString(string strResourceName)
        {
            using (Stream s = Assembly.GetEntryAssembly().GetManifestResourceStream(strResourceName))
            {

                if (s == null)
                {
                    return null;
                }

                byte[] bBuffer = new byte[s.Length];

                if (s.Read(bBuffer, 0, bBuffer.Length) != bBuffer.Length)
                {
                    return null;
                }

                StringBuilder sb = new StringBuilder(bBuffer.Length);

                foreach (byte t in bBuffer)
                {
                    sb.Append((char)t);
                }

                return sb.ToString();
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Gets the grad.
        /// </summary>
        /// <param name="grad">The grad.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static double GetGrad(double grad, double minutes, double seconds)
        {
            return grad + (minutes / 60) + (seconds / 3600);
        }


        /// <summary>
        /// Gets the grad minutes seconds.
        /// </summary>
        /// <param name="grad">The grad.</param>
        /// <returns></returns>
        public static string GetGradMinutesSeconds(double grad)
        {
            double dGrad = Math.Floor(grad);
            double dMin = (grad - dGrad) * 60;
            double dMinGrad = Math.Floor(dMin);
            double dSec = (dMin - dMinGrad) * 60;

            return string.Format("{0}°{1}'{2:F}''", dGrad, dMinGrad, dSec);
        }

    } // end static public class Tools



    /// <summary>
    /// Helper For A MessageBox.
    /// </summary>
    public static class MB
    {

        /// <summary>
        /// Nots the yet implemented.
        /// </summary>
        /// <param name="strCallerName">Name of the string caller.</param>
        public static void NotYetImplemented([CallerMemberName]string strCallerName = null)
        {
            MessageBox.Show(string.Format("{0} reported :\nNotYetImplemented ...", strCallerName), Tool.ProductTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="strErrorText"></param>
        //static public void Error(string strErrorText)
        //{
        //    MessageBox.Show(strErrorText, Tool.ProductTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        //}


        /// <summary>
        /// Errors the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="strCallerName">Name of the string caller.</param>
        public static void Error(Exception ex, [CallerMemberName]string strCallerName = null)
        {
            string strMessage = string.Format("{0} reported:\n{1}", strCallerName, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            MessageBox.Show(strMessage, Tool.ProductTitle, MessageBoxButton.OK, MessageBoxImage.Error);
        }


        /// <summary>
        /// Warnings the specified string information text.
        /// </summary>
        /// <param name="strInformationText">The string information text.</param>
        public static void Warning(string strInformationText)
        {
            MessageBox.Show(strInformationText, Tool.ProductTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }


        /// <summary>
        /// Warnings the specified string format.
        /// </summary>
        /// <param name="strFormat">The string format.</param>
        /// <param name="param">The parameter.</param>
        public static void Warning(string strFormat, params object[] param)
        {
            MessageBox.Show(string.Format(strFormat, param), Tool.ProductTitle, MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }


        /// <summary>
        /// Informations the specified string information text.
        /// </summary>
        /// <param name="strInformationText">The string information text.</param>
        public static void Information(string strInformationText)
        {
            MessageBox.Show(strInformationText, Tool.ProductTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }


        /// <summary>
        /// Informations the specified string format.
        /// </summary>
        /// <param name="strFormat">The string format.</param>
        /// <param name="param">The parameter.</param>
        public static void Information(string strFormat, params object[] param)
        {
            MessageBox.Show(string.Format(strFormat, param), Tool.ProductTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }


        /// <summary>
        /// Heres the i am.
        /// </summary>
        /// <param name="strCallerName">Name of the string caller.</param>
        [Conditional("DEBUG")]
        public static void HereIAm([CallerMemberName]string strCallerName = null)
        {
            MessageBox.Show(string.Format("Here I'am:\n{0}", strCallerName), Tool.ProductTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        }

    } // end static public class MB
}
