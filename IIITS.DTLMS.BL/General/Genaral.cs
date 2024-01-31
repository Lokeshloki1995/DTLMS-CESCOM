using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using IIITS.DAL;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.IO;
using ClosedXML.Excel;
using System.Web;
using System.Linq;

namespace IIITS.DTLMS.BL
{
    public static class Genaral 
    {
        public static CustOledbConnection objCon = new CustOledbConnection(Constants.Password);
        public static bool IsLoggedUserAdmin = false;
        public enum RecursiveDays
        {
            Monday = 1, Tuesday = 2, Wednesday = 3, Thursday = 4, Friday = 5, Saturday = 6, Sunday = 7

        }


        public static void getexcel(DataTable dt, List<string> listtoRemove,string filename,string PageTitle)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                   // string[] arrAlpha = Genaral.getalpha();

                  
                    

                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        if (listtoRemove[0] != "")
                        {
                            foreach (var index in listtoRemove)
                            {
                                dt.Columns.Remove(index);
                            }
                        }

                        //for (int index = 0; index < listtoRemove.Count-1; index++)
                        //{
                        //    // DataColumn dc = dt.Columns[i];
                        //    dt.Columns.RemoveAt(index);

                        //    //if (listtoRemove.Contains(dc.ColumnName.ToUpper()))
                        //    //{
                        //    //    dt.Columns.Remove(dc);
                        //    //}
                        //}
                        int count = dt.Columns.Count;

                      



                        //var rangehead = wb.Worksheet(1).Range("A1:" + rngend + "1");
                        wb.Worksheets.Add(dt, "sheet1");

                        wb.Worksheet(1).Row(1).InsertRowsAbove(3);
                        var sMergeRange = wb.Worksheet(1).Cell(1, count).Address.ColumnLetter;
                       // string sMergeRange = arrAlpha[dt.Columns.Count - 1];

                        var rangehead = wb.Worksheet(1).Range("A1:" + sMergeRange + "1");
                        rangehead.Merge().Style.Font.SetBold().Font.FontSize = 10;
                        rangehead.Merge().Style.Fill.BackgroundColor = XLColor.AliceBlue;
                        rangehead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangehead.SetValue("Chamundeswhari Electricity Supply Board, (CESC Mysore)");

                        //page title
                        var rangeReporthead = wb.Worksheet(1).Range("A2:" + sMergeRange + "2");
                        rangeReporthead.Merge().Style.Font.SetBold().Font.FontSize = 12;
                        rangeReporthead.Merge().Style.Fill.BackgroundColor = XLColor.AirForceBlue;
                        rangeReporthead.Merge().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                        rangeReporthead.SetValue("" + PageTitle);
                        // wb.Worksheet(1).Cell(3, 16).Value = DateTime.Now;

                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.Buffer = true;
                        HttpContext.Current.Response.Charset = "";
                        // string FileName = "CregAbstract " + DateTime.Now + ".xls";
                        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + filename);

                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
                            HttpContext.Current.Response.Flush();
                            HttpContext.Current.Response.End();
                        }
                    }
                }
                else
                {
                   
                }
            }
            catch (Exception ex)
            {
                
            }

        }
       


       
        public static string Encrypt(string pwd)
        {
            //this will return the encrypted string
            int n, i;
            string temp;
            temp = "";
            n = pwd.Length;
            for (i = 0; i < n; i++)
            {
                temp = temp + (char)((int)pwd[i] + 123);
            }
            //temp[i]='\0';
            return (temp);
        }

        public static string Decrypt(string pwd)
        {
            //this will return the encrypted string
            int n, i;
            string temp;
            temp = "";
            n = pwd.Length;
            for (i = 0; i < n; i++)
            {
                temp = temp + (char)((int)pwd[i] - 123);
            }
            //temp[i]='\0';
            return (temp);
        }

        #region Encryption Decryption Logic for ITICKET url
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;
        public static string EncryptTckt(string plainText, string encryptionKey)        {
            encryptionKey = "IIITSITICKET";
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();            var ivStringBytes = Generate256BitsOfRandomEntropy();            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);            using (var password = new Rfc2898DeriveBytes(encryptionKey, saltStringBytes, DerivationIterations))            {                var keyBytes = password.GetBytes(Keysize / 8);                using (var symmetricKey = new RijndaelManaged())                {                    symmetricKey.BlockSize = 256;                    symmetricKey.Mode = CipherMode.CBC;                    symmetricKey.Padding = PaddingMode.PKCS7;                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))                    {                        using (var memoryStream = new MemoryStream())                        {                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))                            {                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();                                memoryStream.Close();                                cryptoStream.Close();                                return Convert.ToBase64String(cipherTextBytes);                            }                        }                    }                }            }        }

        public static string DecryptTckt(string cipherText, string encryptionKey)
        {
            encryptionKey = "IIITSITICKET";
            // As + is getting replaced with ' ' string while passing through the url so replacing the ' ' string with + again
            cipherText = cipherText.Replace(' ', '+');
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(encryptionKey, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            using (var streamReader = new StreamReader(cryptoStream, Encoding.UTF8))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }


        // Generating random 256 bits value
        private static byte[] Generate256BitsOfRandomEntropy()        {            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);            }            return randomBytes;        }
        #endregion`

        public static string EncryptMMS(string pwd)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[pwd.Length];
            encode = Encoding.UTF8.GetBytes(pwd);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        /// <summary>
        /// //Public Function Load_Combo(ByVal Qry As String, ByRef Cmb As Windows.Forms.ComboBox)
        /// </summary>
        /// <param name="Qry"></param>
        /// <param name="cmb"></param>
        /// 
        public static void load_listBox_Web(string strqry, System.Web.UI.WebControls.ListBox lstBox)
        {
            OleDbDataReader drlist = objCon.Fetch(strqry);
            lstBox.Items.Clear();
            while (drlist.Read())
            {
                lstBox.Items.Add(drlist.GetValue(0).ToString());
            }
        }


        public static void Load_Combo(string Qry, System.Web.UI.WebControls.DropDownList cmb)
        {
            Load_Combo(Qry, "", cmb);
        }

        public static void Load_Combo(string Qry, string strSelect, System.Web.UI.WebControls.DropDownList cmb)
        {
     
            OleDbDataReader reader;
            if (objCon.Con.State == ConnectionState.Closed)
                objCon.Con.Open();
            reader = objCon.Fetch(Qry);
            cmb.Items.Clear();
            if (strSelect.Length > 0)
            {
                cmb.Items.Add(strSelect);

            }
            while (reader.Read() == true)
            {
                System.Web.UI.WebControls.ListItem itm = new System.Web.UI.WebControls.ListItem();
                itm.Value = Get_Reader_res(reader, 0);
                itm.Text = Get_Reader_res(reader, 1);


                cmb.Items.Add(itm);

            }

            reader.Close();
        }

        // Function to return true if leavetype provided is optional esle return false
        public static bool is_logged(string sessionUserid)
        {
            if (sessionUserid.Equals(string.Empty))
            {
                return (false);

            }
            else
            {
                return (true);
            }
        }

        /// <summary>
        /// strQry should have only one column and one result
        /// </summary>
        /// <param name="strQry"></param>
        /// <returns></returns>
        public static string get_value(string strQry)
        {
            return (objCon.get_value(strQry));
        }

        public static string Get_Reader_res(OleDbDataReader rd, int index)
        {

            string str = "";
            if (rd.IsDBNull(index) == false)
            {
                if (rd.GetDataTypeName(index) == "DBTYPE_NUMERIC")
                {
                    str = Convert.ToString(rd.GetDecimal(index));
                }
                if (rd.GetDataTypeName(index) == "DBTYPE_VARCHAR")
                {
                    str = Convert.ToString(rd.GetString(index));
                }
                if (rd.GetDataTypeName(index) == "DBTYPE_DBTIMESTAMP")
                {
                    str = Convert.ToString(rd.GetDateTime(index));
                }
                if (rd.GetDataTypeName(index) == "DBTYPE_VARNUMERIC")
                {
                    str = Convert.ToString(rd.GetDecimal(index));
                }
                if (rd.GetDataTypeName(index) == "DBTYPE_WVARCHAR")
                {
                    str = Convert.ToString(rd.GetString(index));
                }


            }
            else
            {
                str = string.Empty;
            }

            return (str);

        }


        /// <summary>
        /// Function to Load Checkbox List
        /// </summary>
        /// <param name="strQuery"></param>
        /// <param name="chklst"></param>
        public static void Load_Checkbox(string strQuery, CheckBoxList chklst, string strText, string strValue)
        {
            try
            {
                OleDbDataAdapter da = new OleDbDataAdapter(strQuery, objCon.Con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                chklst.DataSource = ds;
                chklst.DataTextField = strText;
                chklst.DataValueField = strValue;
                chklst.DataBind();
            }
            catch (Exception ex)
            {

            }

        }


        public static void Load_CheckboxList(string strNameColumn, string strValueColumn,
           string strQry, System.Web.UI.WebControls.CheckBoxList checkboxlist)
        {
            if (strNameColumn.Trim().Equals(string.Empty) || strNameColumn.Trim().Equals(string.Empty)
                || strNameColumn.Trim().Equals(string.Empty) ||
                strNameColumn.Trim().Equals(string.Empty) || strNameColumn.Trim().Equals(string.Empty))
            {
                throw new ArgumentNullException("verify parameters");
            }
            OleDbDataReader drlist = objCon.Fetch(strQry);
            while (drlist.Read())
            {
                ListItem item = new ListItem(drlist.GetValue(drlist.GetOrdinal(strNameColumn)).ToString(), drlist.GetValue(drlist.GetOrdinal(strValueColumn)).ToString());
                checkboxlist.Items.Add(item);
            }

        }

        // To Encrypt URL
        public static string UrlEncrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
			0x49,
			0x76,
			0x61,
			0x6e,
			0x20,
			0x4d,
			0x65,
			0x64,
			0x76,
			0x65,
			0x64,
			0x65,
			0x76
		});
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        // To Decrypt URL
        public static string UrlDecrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
			0x49,
			0x76,
			0x61,
			0x6e,
			0x20,
			0x4d,
			0x65,
			0x64,
			0x76,
			0x65,
			0x64,
			0x65,
			0x76
		});
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }



        //public static void Load_Combo_From_String(string strValue, char chrSeperator,string strDefaultValue, System.Web.UI.WebControls.DropDownList cmb)
        //{
        //    string[] arrValues = strValue.Split(chrSeperator);

        //    cmb.Items.Clear();
        //    cmb.Items.Add(strDefaultValue);
        //    if (arrValues.Length > 0)
        //    {
        //        for (int i = 0; i < arrValues.Length; i++)
        //        {
        //            System.Web.UI.WebControls.ListItem itm = new System.Web.UI.WebControls.ListItem();
        //            itm.Value = arrValues.GetValue(i).ToString();
        //            itm.Text = arrValues.GetValue(i).ToString();
        //            cmb.Items.Add(itm);
        //        }
        //    }

        //}

        public static void Load_Table(string Qry, System.Web.UI.WebControls.Table Tab)
        {
            int i, j;
            System.Web.UI.WebControls.TableRow row = new System.Web.UI.WebControls.TableRow();
            OleDbDataReader rd;

            rd = objCon.Fetch(Qry);
            i = j = 0;
            {
                System.Web.UI.WebControls.TableCell cel = new System.Web.UI.WebControls.TableCell();
                row.Cells.Add(cel);
            }
            while (i < rd.FieldCount)
            {
                System.Web.UI.WebControls.TableCell cel = new System.Web.UI.WebControls.TableCell();

                cel.Text = rd.GetName(i);
                cel.BackColor = System.Drawing.Color.AliceBlue;
                cel.ForeColor = System.Drawing.Color.Red;
                cel.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Middle;
                cel.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                row.Cells.Add(cel);
                i++;
            }


            Tab.Rows.Add(row);  //Header to the table

            //here data will be added to the table

            while (rd.Read() == true)
            {
                System.Web.UI.WebControls.TableRow row1 = new System.Web.UI.WebControls.TableRow();
                i = 0;
                j++;
                {
                    //add a select button here with javascript emabed..
                    TableCell cel = new TableCell();
                    System.Web.UI.WebControls.Button btn = new System.Web.UI.WebControls.Button();
                    btn.ID = j.ToString();
                    btn.Text = "Select";
                    btn.Attributes.Add("onclick", "javascript:GetRowValue('" + Get_Reader_res(rd, 0) + "')");
                    cel.Controls.Add(btn);
                    row1.Cells.Add(cel);
                }
                while (i < rd.FieldCount)
                {
                    System.Web.UI.WebControls.TableCell cel = new System.Web.UI.WebControls.TableCell();

                    if (rd.IsDBNull(i) == false)
                    {
                        cel.Text = Get_Reader_res(rd, i);

                    }
                    else
                    {
                        cel.Text = " ";
                    }

                    if (j % 2 == 0)
                    {
                        cel.BackColor = System.Drawing.Color.White;
                        cel.ForeColor = System.Drawing.Color.Blue;
                    }
                    else
                    {
                        cel.BackColor = System.Drawing.Color.SeaShell;
                        cel.ForeColor = System.Drawing.Color.Blue;
                    }

                    cel.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Middle;
                    cel.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                    row1.Cells.Add(cel);
                    i++;
                }


                Tab.Rows.Add(row1);

            }

            Tab.Height = 36 * j;
            Tab.BorderColor = System.Drawing.Color.YellowGreen;
        }



        ///////////////////// Date Validation /////////////////////
        //public string isValidDate(string sDate) //Input date should be in dd/MM/yyyy Format
        //{
        //    string[] sArry = null;
        //    int i;
        //    long  minYear = 1900;
        //    long  maxYear = 2100;
        //    try
        //    {
        //        sDate = sDate.Replace('-', '/');
        //        sDate = sDate.Replace('.', '/');
               
        //        if (sDate.InStr(sDate, "/") == 0)
        //        {
        //            //isValidDate = "Please Enter Date in dd/MM/yyyy format"
        //            functionReturnValue = "Not a valid Date format : Date should be in dd/MM/yyyy format";
        //            break; // TODO: might not be correct. Was : Exit Try
        //        }

        //        sArry = Strings.Split(sDate, "/");
        //        if (Information.UBound(sArry) != 2)
        //        {
        //            //isValidDate = "Please Enter Date in dd/MM/yyyy format"
        //            functionReturnValue = "Not a valid Date format : Date should be in dd/MM/yyyy format";
        //            break; // TODO: might not be correct. Was : Exit Try
        //        }
        //        sArry[2] = Strings.LTrim(sArry[2]);
        //        sArry[2] = Strings.RTrim(sArry[2]);
        //        //If time is included in year
        //        if (sArry[2].Contains(" "))
        //        {
        //            sArry[2] = Strings.RTrim(sArry[2].Substring(0, Strings.InStr(sArry[2], " ")));
        //        }
        //        //Month
        //        if (sArry[1] > 12)
        //        {
        //            //isValidDate = "Not A Valid Month"
        //            functionReturnValue = "Not a valid Date format : Date should be in dd/MM/yyyy format";
        //            break; // TODO: might not be correct. Was : Exit Try
        //        }
        //        if (Conversion.Val(sArry[1]) == 0)
        //        {
        //            //isValidDate = "Not A Valid Month"
        //            functionReturnValue = "Not a valid Date format : Date should be in dd/MM/yyyy format";
        //            break; // TODO: might not be correct. Was : Exit Try
        //        }
        //        //Year
        //        if ((sArry[2].Length != 4) | (sArry[2] == 0) | (sArry[2] < minYear) | (sArry[2] > maxYear))
        //        {
        //            functionReturnValue = "Please enter a valid 4 digit year between " + minYear + " and " + maxYear;
        //            break; // TODO: might not be correct. Was : Exit Try
        //        }
        //        if (sArry[i] > System.DateTime.DaysInMonth(sArry[2], sArry[1]))
        //        {
        //            //isValidDate = "Not a Valid Day"
        //            functionReturnValue = "Not a valid Date format : Date should be in dd/MM/yyyy format";
        //            break; // TODO: might not be correct. Was : Exit Try
        //        }
        //        if (Conversion.Val(sArry[i]) == 0)
        //        {
        //            //isValidDate = "Not a Valid Day"
        //            functionReturnValue = "Not a valid Date format : Date should be in dd/MM/yyyy format";
        //            break; // TODO: might not be correct. Was : Exit Try
        //        }
        //        functionReturnValue = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Not a valid Date format : Date should be in dd/MM/yyyy format";
        //    }
        //    return functionReturnValue;
        //}

        //public string DateComparison(string sDate1, string sDate2)
        //{

        //    //sDate1 = Replace(sDate1, "-", "/") : sDate1 = Replace(sDate1, ".", "/") : sDate1 = Replace(sDate1, "\", "/")
        //    sDate1 = sDate1.Replace('-', '/');
        //    sDate1 = sDate1.Replace('.', '/');

        //    sDate2 = sDate2.Replace('-', '/');
        //    sDate2 = sDate2.Replace('.', '/');

        //    string functionReturnValue = null;//Input date should be in dd/MM/yyyy Format
        //    string[] sArry = null;
        //    Int16 i = default(Int16);
        //    try
        //    {
        //        sDate1 = Strings.Replace(sDate1, "-", "/");
        //        sDate1 = Strings.Replace(sDate1, ".", "/");
        //        sDate1 = Strings.Replace(sDate1, "\\", "/");
        //        sDate2 = Strings.Replace(sDate2, "-", "/");
        //        sDate2 = Strings.Replace(sDate2, ".", "/");
        //        sDate2 = Strings.Replace(sDate2, "\\", "/");
        //        if (isValidDate(sDate1) != "True")
        //        {
        //            functionReturnValue = "False";
        //        }
        //        if (isValidDate(sDate2) != "True")
        //        {
        //            functionReturnValue = "False";
        //        }

        //        long iFrom = 0;
        //        long iTo = 0;
        //        sArry = Strings.Split(sDate1, "/");
        //        if (Information.UBound(sArry) != 2)
        //        {
        //            functionReturnValue = "False";
        //            break; // TODO: might not be correct. Was : Exit Try
        //        }
        //        //If time is included
        //        if (sArry[2].Contains(" "))
        //        {
        //            sArry[2] = Strings.RTrim(sArry[2].Substring(0, Strings.InStr(sArry[2], " ")));
        //        }
        //        iFrom = sArry[2] + sArry[1] + sArry[0];

        //        sArry = Strings.Split(sDate2, "/");
        //        if (Information.UBound(sArry) != 2)
        //        {
        //            functionReturnValue = "False";
        //            break; // TODO: might not be correct. Was : Exit Try
        //        }
        //        //If time is included
        //        if (sArry[2].Contains(" "))
        //        {
        //            sArry[2] = Strings.RTrim(sArry[2].Substring(0, Strings.InStr(sArry[2], " ")));
        //        }
        //        iTo = sArry[2] + sArry[1] + sArry[0];
        //        if (iFrom > iTo)
        //        {
        //            functionReturnValue = 1;
        //        }
        //        if (iTo > iFrom)
        //        {
        //            functionReturnValue = 2;
        //        }
        //        if (iFrom == iTo)
        //        {
        //            functionReturnValue = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return "False";
        //    }
        //    return functionReturnValue;
        //}



        public static string DateComparision(string sDate1, string sDate2, bool bCurrentDate, bool bEqual)
        {

            DateTime dDate1 = DateTime.ParseExact(sDate1.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime? dDate2 = null ;
            if (sDate2 != "")
            {
                 dDate2 = DateTime.ParseExact(sDate2.Replace('-', '/'), "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }

            string sReturn = string.Empty;
            if (bCurrentDate == true)
            {
                if ((dDate1  > Convert.ToDateTime(DateTime.Now.Date.ToShortDateString().Replace('-', '/'))))
                {

                    sReturn = "1";
                }
                if ((dDate1  < Convert.ToDateTime(DateTime.Now.Date.ToShortDateString().Replace('-', '/'))))
                {

                    sReturn = "2";
                }
                if (bEqual == true)
                {
                    if (dDate1.ToString("dd/MM/yyyy").Replace('-', '/') == DateTime.Now.Date.ToString("dd/MM/yyyy").Replace('-', '/'))
                    {
                        sReturn = "0";
                    }
                }

            }
            else
            {
                if ((dDate1  > dDate2))
                {

                    sReturn = "1";
                }
                if ((dDate1  < dDate2))
                {

                    sReturn = "2";
                }

                if (dDate1  == dDate2 )
                {
                    sReturn = "0";
                }

            }
            return sReturn;

        }

        public static string[] getalpha()
        {
            string[] arrAlpha = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z","AA","AB","AC" , "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK" };

            return arrAlpha;
        }

        public static string ERPUrlDecrypt(string cipherText)
        {
            string EncryptionKey = "IIITSCESCERP2022";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49,
            0x76,
            0x61,
            0x6e,
            0x20,
            0x4d,
            0x65,
            0x64,
            0x76,
            0x65,
            0x64,
            0x65,
            0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public static string ERPUrlEncrypt(string clearText)
        {
            string EncryptionKey = "IIITSCESCERP2022";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49,
            0x76,
            0x61,
            0x6e,
            0x20,
            0x4d,
            0x65,
            0x64,
            0x76,
            0x65,
            0x64,
            0x65,
            0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }


        public static string DateValidation(string sDate)
        {
            string sReturn = string.Empty;

            //string Date = @"^([1-9]|0[1-9]|1[0-2])[- / .]([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$";
            string Date = @"^([1-9]|0[1-9]|1[0-9]|2[0-9]|3[0-1])[- / .]([1-9]|0[1-9]|1[0-2])[- / .](1[9][0-9][0-9]|2[0][0-9][0-9])$";
            System.Text.RegularExpressions.Regex r1 = new System.Text.RegularExpressions.Regex(Date);
            if (!r1.IsMatch(sDate))
            {
                sReturn = "Please Enter a valid date in the format (dd/mm/yyyy)";

            }
            string DatePattern = @"^[a-zA-Z0-9 !@#$%^&*)(]{1,20}$";
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(DatePattern);
            if (r.IsMatch(sDate))
            {
                sReturn = "Please Enter a valid date in the format (dd/mm/yyyy)";

            }
            return sReturn;

        }

        public static void GeneralLog(string sClient, string sUserID, string LogType)
        {
            string sFolderPath = Convert.ToString(ConfigurationSettings.AppSettings["LOGS"]) + DateTime.Now.ToString("yyyyMM");
            try
            {
                if (!Directory.Exists(sFolderPath))
                {
                    Directory.CreateDirectory(sFolderPath);
                }

                int NoofTimes = 0;

                LOOP:
                try
                {
                    if (LogType == "LOGIN")
                    {
                        string sPath = sFolderPath + "//" + "LogIn " + DateTime.Now.ToString("yyyyMMdd") + "-LogIn.txt";
                        File.AppendAllText(sPath, Environment.NewLine + "EVENT_NAME -- LOGIN | IP_ADDRESS -- " + sClient + " | DEVICE_TYPE -- WEB | USER_ID --" + sUserID + " | ENTRY_DATE -- " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
                    }
                    else
                    {
                        string sPath = sFolderPath + "//" + "LogOut " + DateTime.Now.ToString("yyyyMMdd") + "-LogOut.txt";
                        File.AppendAllText(sPath, Environment.NewLine + "EVENT_NAME -- LOGOUT | IP_ADDRESS -- " + sClient + " | DEVICE_TYPE -- WEB | USER_ID --" + sUserID + " | ENTRY_DATE -- " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
                    }
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(250);
                    NoofTimes++;
                    if (NoofTimes <= 3)
                    {
                        goto LOOP;
                    }
                    else
                    {
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "Genaral", "GeneralLog");
            }
        }

        public static DataTable GetConfiguration()
        {
            DataTable dt = new DataTable();
            string query = string.Empty;
            try
            {
                query = "SELECT * FROM TBLCONFIGURATION";
                dt = objCon.getDataTable(query);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "Genaral", "GetConfiguration");
            }
            return dt;
        }

        public static string GetClientIp()
        {
            try
            {
                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                return sClientIP;

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "Genaral", "GetClientIp");
                return "";
            }
        }

        public static void PasswordChangeLog(string sClient, string sUserID)
        {
            string sFolderPath = Convert.ToString(ConfigurationSettings.AppSettings["LOGS"]) + DateTime.Now.ToString("yyyyMM");
            try
            {
                if (!Directory.Exists(sFolderPath))
                {
                    Directory.CreateDirectory(sFolderPath);
                }

                string sPath = sFolderPath + "//" + "PasswordChange " + DateTime.Now.ToString("yyyyMMdd") + "-PasswordChange.txt";
                File.AppendAllText(sPath, Environment.NewLine + "EVENT_NAME -- Password Change | IP_ADDRESS -- " + sClient + " | DEVICE_TYPE -- WEB | USER_ID --" + sUserID + " | ENTRY_DATE -- " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "Genaral", "PasswordChangeLog");
            }
        }

        public static void TransactionLog(string sClient, string sUserID, string sTransactionName)
        {
            string sFolderPath = Convert.ToString(ConfigurationSettings.AppSettings["LOGS"]) + DateTime.Now.ToString("yyyyMM");
            try
            {
                if (!Directory.Exists(sFolderPath))
                {
                    Directory.CreateDirectory(sFolderPath);
                }
                int NoofTimes = 0;
                LOOP:
                try
                {
                    string sPath = sFolderPath + "//" + "Transaction " + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    File.AppendAllText(sPath, Environment.NewLine + "EVENT_NAME -- " + sTransactionName + " Transaction | IP_ADDRESS -- " + sClient + " | DEVICE_TYPE -- WEB | USER_ID --" + sUserID + " | ENTRY_DATE -- " + DateTime.Now.ToString("yyyyMMdd hh:mm:ss"));
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(250);
                    NoofTimes++;
                    if (NoofTimes <= 3)
                    {
                        goto LOOP;
                    }
                    else
                    {
                        throw ex;
                    }
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "Genaral", "TransactionLog");
            }
        }

        public static bool CompareLogin(string sActualPassword, string sGivenPassword)
        {
            try
            {
                string sEncGivenPassword = string.Empty;

                byte[] hashbbytes = Convert.FromBase64String(sActualPassword);
                // Take the salt out of string
                byte[] salt = new byte[16];
                Array.Copy(hashbbytes, 0, salt, 0, 16);
                // Hash the user input pw with salt
                var pwdwithsalt = new Rfc2898DeriveBytes(sGivenPassword, salt, 10000);
                //put the hashed input in a byte array to compare with byte-byte
                byte[] hash = pwdwithsalt.GetBytes(20);
                sEncGivenPassword = Convert.ToBase64String(hash);

                byte[] PwdByte = new byte[36];
                Array.Copy(salt, 0, PwdByte, 0, 16);
                Array.Copy(hash, 0, PwdByte, 16, 20);
                string finalsavepwd = Convert.ToBase64String(PwdByte);
                sEncGivenPassword = finalsavepwd;

                int ok = 1;
                for (int i = 0; i < 20; i++)

                    if (hashbbytes[i + 16] != hash[i])
                        ok = 0;

                if (ok == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message, "General", "CompareLogin");
                return false;
            }
        }

        public static string EncryptPassword(string sPassword)
        {
            // hash is 20 bytes, and the salt 16.
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            //HASH AND SALT IT USING PBKDF2
            var pwwithsalt = new Rfc2898DeriveBytes(sPassword, salt, 10000);
            //place the string in the byte array 
            byte[] hash = pwwithsalt.GetBytes(20);
            // make new byte array where to store the hashed password + salt 
            // why 36 cos hash is 20 bytes, and the salt 16.
            byte[] hashbytes = new byte[36];
            // place the hash and password at respective places 
            Array.Copy(salt, 0, hashbytes, 0, 16);
            Array.Copy(hash, 0, hashbytes, 16, 20);
            string finalsavepwd = Convert.ToBase64String(hashbytes);
            return finalsavepwd;
        }

    }
}
