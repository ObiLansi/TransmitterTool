using System;
using System.Windows.Media;



namespace TransmitterTool.Extensions
{
    /// <summary>
    /// Eine Erweiterungsklasse f�r unseren lieblichen String.
    /// </summary>
    static public class StringExtension
    {

        /// <summary>
        /// Removes the quotation.
        /// </summary>
        /// <param name="strContent">Content of the STR.</param>
        /// <returns></returns>
        static public string RemoveQuotation(this string strContent)
        {
            string strTemp = strContent.Trim();

            if (strTemp.StartsWith("\"") && strTemp.EndsWith("\""))
            {
                return strTemp.Substring(1, strContent.Length - 2);
            }

            return strContent;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Liefert zur�ck ob ein String null oder dessen L�nge 0 ist.
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        static public bool IsEmpty(this string strContent)
        {
            return string.IsNullOrEmpty(strContent);
        }


        /// <summary>
        /// Liefert zur�ck ob ein String nicht null oder dessen L�nge > 0 ist.
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        static public bool IsNotEmpty(this string strContent)
        {
            return !string.IsNullOrEmpty(strContent);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Capitalizes the specified string content.
        /// </summary>
        /// <param name="strContent">Content of the string.</param>
        /// <returns></returns>
        static public string Capitalize(this string strContent)
        {
            char[] aData = strContent.ToLower().ToCharArray();

            for (int iCounter = 0; iCounter < aData.Length; iCounter++)
            {
                if (iCounter == 0)
                {
                    if (Char.IsLetter(aData[iCounter]))
                    {
                        aData[iCounter] -= (char)0x20;
                    }
                }
                else
                {
                    if (Char.IsLetter(aData[iCounter - 1]) == false && aData[iCounter - 1] != '\'')
                    {
                        if (Char.IsLetter(aData[iCounter]))
                        {
                            aData[iCounter] -= (char)0x20;
                        }
                    }
                }
            }

            return new string(aData);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /// <summary>
        /// Liefert aus einem String wie z.b. "#FFAACC" den Farbwert als Color Objekt zur�ck.
        /// </summary>
        /// <param name="strColor"></param>
        /// <param name="cDefault"></param>
        /// <returns></returns>
        /// <remarks>Es k�nnten auch die .NET symbolischen Farbnamen wie "SlateBlue" �bergeben werden.</remarks>
        static public Color ToColor(this string strColor, Color cDefault)
        {
            if (!string.IsNullOrEmpty(strColor))
            {
                if (strColor[0] == '#')
                {
                    try
                    {
                        return (Color)ColorConverter.ConvertFromString(strColor);
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return cDefault;
        }

    } // end static public class StringExtension
}
