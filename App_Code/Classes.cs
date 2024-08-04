using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using System.Windows;
using System.Diagnostics;
using Stripe;
using AjaxControlToolkit;
using System.Net;
using System.Net.Mail;
using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using OtpNet;

public class Clothes
{
    private string _id;
    private string _name;
    private int _quantity;
    private decimal _price;
    private string _overview;
    private char _gender;
    private int _category_id;
    private string _link;
    private DateTime _dateAdded;

    public string id { get { return _id; } }
    public string name { get { return _name; } set { _name = value; } }
    public int quantity { get { return _quantity; } set { _quantity = value; } }
    public decimal price { get { return _price; } set { _price = value; } }
    public string overview { get { return _overview; } set { _overview = value; } }
    public char gender { get { return _gender; } set { _gender = value; } }
    public int category_id { get { return _category_id; } set { _category_id = value; } }
    public string link { get { return _link; } }
    public DateTime DateAdded { get => _dateAdded; }

    public Clothes(string id, string name, int quantity, decimal price, string overview, char gender, int category_id, string link, DateTime dateAdded)
    {
        this._id = id;
        this._name = name;
        this._quantity = quantity;
        this._price = price;
        this._overview = overview;
        this._gender = gender;
        this._category_id = category_id;
        this._link = link;
        this._dateAdded = dateAdded;
    }

    public static Clothes getClothesID(string clothesID)
    {
        Clothes clothingObj;
        int quantity, category_id;
        string id, name, overview, link;
        decimal price;
        char gender;
        DateTime dateAdded;

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
        SqlCommand cmd = new SqlCommand("select * from Clothes WHERE ID = @cid", conn);
        cmd.Parameters.AddWithValue("@cid", clothesID);

        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            id = dr["Id"].ToString();
            name = dr["name"].ToString();
            quantity = int.Parse(dr["quantity"].ToString());
            price = decimal.Parse(dr["price"].ToString());
            overview = dr["overview"].ToString();
            gender = char.Parse(dr["gender"].ToString());
            category_id = int.Parse(dr["category_id"].ToString());
            link = dr["link"].ToString();
            dateAdded = DateTime.Parse(dr["dateAdded"].ToString());

            clothingObj = new Clothes(id, name, quantity, price, overview, gender, category_id, link, dateAdded);
        }
        else
        {
            clothingObj = null;
        }

        conn.Close();
        dr.Close();
        dr.Dispose();

        return clothingObj;
    }

    public class Cart
    {
        public string item_name;
        public int item_quantity;
        public double item_price;
        public string item_color;
        public string item_size;
        public string clothes_id;

        public Cart(string item_name, int item_quantity, double item_price, string item_color, string item_size, string clothes_id)
        {
            this.item_name = item_name;
            this.item_quantity = item_quantity;
            this.item_price = item_price;
            this.item_color = item_color;
            this.item_size = item_size;
            this.clothes_id = clothes_id;
        }
    }
}

public class Account
{
    private string _id;
    private string _firstname;
    private string _lastname;
    private string _email;
    private bool _emailConfirmed;
    private bool _isAdmin;
    private string _password;
    private string _mobilenumber;
    private bool _mfaEnabled;
    private string _secret_key;
    private string _adress1;
    private string _adress2;
    private string _zipcode;

    public string id { get { return _id; } }
    public string firstname { get { return _firstname; } set { _firstname = value; } }
    public string lastname { get { return _lastname; } set { _lastname = value; } }
    public string email { get { return _email; } set { _email = value; } }
    public bool emailConfirmed { get { return _emailConfirmed; } set { _emailConfirmed = value; } }
    public bool isAdmin { get { return _isAdmin; } }
    public string password { get { return _password; } }
    public string mobilenumber { get { return _mobilenumber; } set { _mobilenumber = value; } }
    public bool mfaEnabled { get { return _mfaEnabled; } set { _mfaEnabled = value; } }
    public string secret_key { get { return _secret_key; } set { _secret_key = value; } }
    public string adress1 { get { return _adress1; } set { _adress1 = value; } }
    public string adress2 { get { return _adress2; } set { _adress2 = value; } }
    public string zipcode { get { return _zipcode; } set { _zipcode = value; } }

    public Account(string id, string firstname, string lastname, string email, bool emailConfirmed, bool isAdmin, string password, string mobilenumber, bool mfaEnabled, string secret_key, string adress1, string adress2, string zipcode)
    {
        this._id = id;
        this._firstname = firstname;
        this._lastname = lastname;
        this._email = email;
        this._emailConfirmed = emailConfirmed;
        this._isAdmin = isAdmin;
        this._password = password;
        this._mobilenumber = mobilenumber;
        this._mfaEnabled = mfaEnabled;
        this._secret_key = secret_key;
        this._adress1 = adress1;
        this._adress2 = adress2;
        this._zipcode = zipcode;
    }

    public static Account GetAccount(string emailParam)
    {
        Account account = null;

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
        SqlCommand cmd = new SqlCommand("select * from Accounts WHERE email = @email", conn);
        cmd.Parameters.AddWithValue("@email", emailParam);
        conn.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            account = new Account(
                dr["Id"].ToString(),
                dr["first_name"].ToString(),
                dr["last_name"].ToString(),
                dr["email"].ToString(),
                bool.Parse(dr["emailConfirmed"].ToString()),
                bool.Parse(dr["isAdmin"].ToString()),
                dr["password"].ToString(),
                dr["mobile_number"].ToString(),
                bool.Parse(dr["multi_factor_enabled"].ToString()),
                dr["secret_key"].ToString(),
                dr["address1"].ToString(),
                dr["address2"].ToString(),
                dr["zipcode"].ToString()
            );
        }
        conn.Close();
        dr.Close();
        dr.Dispose();
        return account;
    }

    public class SecretKeys
    {
        public string stripe_api_key { get; set; }
    }
}

public class SecretKeys
{
    public string stripe_api_key { get; set; }
    public string google_public { get; set; }
    public string google_secret { get; set; }
    public string ipify { get; set; }
}

public class ForgetPasswordTemplateData {
    [JsonProperty("resetlink")]
    public string resetlink { get; set; }

    //[JsonProperty("name")]
    //public string Name { get; set; }
}
 

public class Function {
    public static string GenerateRandomPassword(int size) {
        // Create a strong random password for user
        Random rand = new Random();
        string possibleChars = "abcdefghijklmnopqrstuvwxyz0123456789QWERTYUIOPASDFGHJKLZXCVBNM!@#$^&*()";
        char[] randomPassword = new char[size];

        for (int i = 0; i < size; i++) {
            randomPassword[i] = possibleChars[rand.Next(possibleChars.Length)];
        }
        return string.Join("", randomPassword);
    }
   public static string DecryptEmailToken(string textToDecrypt) {
       try {
           //string  = "6+PXxVWlBqcUnIdqsMyUHA==";
           string ToReturn = "";
           string publickey = "12345678";
           string secretkey = "87654321";
           byte[] privatekeyByte = { };
           privatekeyByte = Encoding.UTF8.GetBytes(secretkey);
           byte[] publickeybyte = { };
           publickeybyte = Encoding.UTF8.GetBytes(publickey);
           MemoryStream ms = null;
           CryptoStream cs = null;
           byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
           inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
           using (DESCryptoServiceProvider des = new DESCryptoServiceProvider()) {
               ms = new MemoryStream();
               cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
               cs.Write(inputbyteArray, 0, inputbyteArray.Length);
               cs.FlushFinalBlock();
               Encoding encoding = Encoding.UTF8;
               ToReturn = encoding.GetString(ms.ToArray());
           }
           return ToReturn;
       } catch (Exception ae) {
           throw new Exception(ae.Message, ae.InnerException);
       }
   }
   public static bool HasEmailTokenExpired(string token, int tokenLifeSpanDays = 3) {
       var data = Convert.FromBase64String(token);
       var tokenCreationDate = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
       return tokenCreationDate < DateTime.UtcNow.AddDays(-tokenLifeSpanDays);
   }
   public static string EncryptEmailToken(string textToEncrypt) {
       try {
           //string  = "WaterWorld";
           string ToReturn = "";
           string publickey = "12345678";
           string secretkey = "87654321";
           byte[] secretkeyByte = { };
           secretkeyByte = Encoding.UTF8.GetBytes(secretkey);
           byte[] publickeybyte = { };
           publickeybyte = Encoding.UTF8.GetBytes(publickey);
           MemoryStream ms = null;
           CryptoStream cs = null;
           byte[] inputbyteArray = Encoding.UTF8.GetBytes(textToEncrypt);
           using (DESCryptoServiceProvider des = new DESCryptoServiceProvider()) {
               ms = new MemoryStream();
               cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
               cs.Write(inputbyteArray, 0, inputbyteArray.Length);
               cs.FlushFinalBlock();
               ToReturn = Convert.ToBase64String(ms.ToArray());
           }
           return ToReturn;
       } catch (Exception ex) {
           throw new Exception(ex.Message, ex.InnerException);
       }
   }
   public static string CreateEmailToken(byte[] arg) {
       var time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
       var key = arg ?? Guid.NewGuid().ToByteArray();
       var token = Convert.ToBase64String(time.Concat(key).ToArray());

       return token;
   }
  // Admin DB
  public static GridView GetAllClothes(GridView gridView)
  {
      DataTable dt = new DataTable();
      using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
      {
          using (SqlCommand command = new SqlCommand("Product_CRUD", connection))
          {
              connection.Open();
              command.Parameters.AddWithValue("@ACTION", "SELECT");
              command.CommandType = CommandType.StoredProcedure;

              using (SqlDataAdapter sql = new SqlDataAdapter())
              {
                  sql.SelectCommand = command;

                  sql.Fill(dt);
              }
              if (dt.Rows.Count > 0)
              {
                  gridView.DataSource = dt;
                  gridView.DataBind();
                  gridView.HeaderRow.TableSection = TableRowSection.TableHeader;
              }
              else
              {
                  dt.Rows.Add(dt.NewRow());
                  gridView.DataSource = dt;
                  gridView.DataBind();
                  gridView.Rows[0].Cells.Clear();
                  gridView.Rows[0].Cells.Add(new TableCell());
                  gridView.Rows[0].Cells[0].ColumnSpan = dt.Columns.Count;
                  gridView.Rows[0].Cells[0].Text = "No Clothes in Database";
                  gridView.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                  gridView.Rows[0].Cells[0].VerticalAlign = VerticalAlign.Middle;
              }
          }
      }
      return gridView;
  }

   public static GridView AdminUpdateClothe(GridView gridView, int rowIndex) {
       try
       {
           using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
           {
               using (SqlCommand sql = new SqlCommand("Product_CRUD", connection))
               {
                   connection.Open();
                   sql.Parameters.AddWithValue("@ACTION", "UPDATE");
                   sql.CommandType = CommandType.StoredProcedure;
                   sql.Parameters.AddWithValue("@id", gridView.DataKeys[rowIndex].Value.ToString());
                   sql.Parameters.AddWithValue("@name", (gridView.Rows[rowIndex].FindControl("TextBox_name") as TextBox).Text.Trim());
                   sql.Parameters.AddWithValue("@quantity", Convert.ToInt32((gridView.Rows[rowIndex].FindControl("TextBox_quantity") as TextBox).Text.Trim()));
                   sql.Parameters.AddWithValue("@price", Convert.ToDouble((gridView.Rows[rowIndex].FindControl("TextBox_price") as TextBox).Text.Trim()));
                   sql.Parameters.AddWithValue("@overview", (gridView.Rows[rowIndex].FindControl("TextBox_Overview") as TextBox).Text.Trim());
                   sql.Parameters.AddWithValue("@gender", (gridView.Rows[rowIndex].FindControl("TextBox_gender") as TextBox).Text.Trim());
                   sql.Parameters.AddWithValue("@category_id", Convert.ToInt32((gridView.Rows[rowIndex].FindControl("TextBox_categoryID") as TextBox).Text.Trim()));
                   sql.Parameters.AddWithValue("@link", (gridView.Rows[rowIndex].FindControl("TextBox_link") as TextBox).Text.Trim());
                   //sql.Parameters.AddWithValue("@", );
                   sql.ExecuteNonQuery();
                   gridView.EditIndex = -1;
                   //gridView = GetAllClothes(gridView);
                   return gridView;
               }
           }
       }
       catch (SqlException)
       {
           return null;
       }
       catch (FormatException)
       {
           return null;
       }
   }
   public static GridView AdminDeleteClothe(GridView gridView, int rowIndex, HttpServerUtility server)
   {
       try
       {
           using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
           {
               using (SqlCommand sql = new SqlCommand("Product_CRUD", connection))
               {
                   connection.Open();
                   string id = gridView.DataKeys[rowIndex].Value.ToString();
                   sql.Parameters.AddWithValue("@ACTION", "DELETE");
                   sql.CommandType = CommandType.StoredProcedure;
                   sql.Parameters.AddWithValue("@id", id);
                   sql.ExecuteNonQuery();

                   // Remove Directory
                   DirectoryInfo di = new DirectoryInfo(server.MapPath(@"/assets/img/_clothing/carousel/" + id + "/"));
                   foreach (FileInfo file in di.GetFiles())
                   {
                       file.Delete();
                   }
                   di.Delete();

                   gridView = GetAllClothes(gridView);
                   return gridView;
               }
           }
       }
       catch (SqlException) { return null; }
   }
   public static GridView AdminInsertClothing(GridView gridView, GridViewCommandEventArgs e, HttpServerUtility server) {
       try
       {
           if (e.CommandName == "Add") {
               using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
               {
                   using (SqlCommand sql = new SqlCommand("Product_CRUD", connection))
                   {
                       connection.Open();
                       sql.Parameters.AddWithValue("@ACTION", "INSERT");
                       sql.CommandType = CommandType.StoredProcedure;
                       string id = Guid.NewGuid().ToString();
                       sql.Parameters.AddWithValue("@id", id);
                       sql.Parameters.AddWithValue("@name", (gridView.HeaderRow.FindControl("Add_Name") as TextBox).Text.Trim());
                       sql.Parameters.AddWithValue("@quantity", Convert.ToInt32((gridView.HeaderRow.FindControl("Add_Quantity") as TextBox).Text.Trim()));
                       sql.Parameters.AddWithValue("@price", Convert.ToDouble((gridView.HeaderRow.FindControl("Add_Price") as TextBox).Text.Trim()));
                       sql.Parameters.AddWithValue("@overview", (gridView.HeaderRow.FindControl("Add_Overview") as TextBox).Text.Trim());
                       sql.Parameters.AddWithValue("@gender", (gridView.HeaderRow.FindControl("Add_Gender") as TextBox).Text.Trim().ToUpper());
                       sql.Parameters.AddWithValue("@category_id", Convert.ToInt32((gridView.HeaderRow.FindControl("Add_CategoryID") as TextBox).Text.Trim()));
                       sql.Parameters.AddWithValue("@link", (gridView.HeaderRow.FindControl("Add_Link") as TextBox).Text.Trim());
                       sql.Parameters.AddWithValue("@today", DateTime.Now);
                       sql.ExecuteNonQuery();

                       // Create directory for images
                       string path = server.MapPath(@"/assets/img/_clothing/carousel/" + id);
                       if (!Directory.Exists(path))
                       {
                           Directory.CreateDirectory(path);
                       }

                       // Save Images
                       StringBuilder files = new StringBuilder();
                       FileUpload images = (gridView.HeaderRow.FindControl("FileUpload_image") as FileUpload);
                       try
                       {
                           if (images.HasFiles)
                           {

                               foreach (var image in images.PostedFiles)
                               {

                                   // Check if "1.jpg" exists, if not rename the image to it - Saving images
                                   if (!System.IO.File.Exists(Path.Combine(path, "1.jpg"))) image.SaveAs(Path.Combine(path, "1.jpg"));
                                   else image.SaveAs(Path.Combine(path, image.FileName));
                               }
                           }
                       }
                       catch (Exception err)
                       {
                           Debug.WriteLine(err);
                           return null;
                       }
                       //return gridView;
                       return GetAllClothes(gridView);
                   }
               }
           }
       }
       catch (SqlException err)
       {
           Debug.WriteLine(err);
           return null;
       }
       catch (FormatException err)
       {
           Debug.WriteLine(err);
           return null;
       }
       return null;
   }
   
    public static int GetDecryptedTokenEmailFromDataBase(string decryptedToken)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);

        conn.Open();

        string checkuser = "SELECT COUNT(*) FROM Accounts WHERE Email = @email";
        SqlCommand com = new SqlCommand(checkuser, conn);
        string email = Encoding.ASCII.GetString(Convert.FromBase64String(decryptedToken)).Substring(8);
        com.Parameters.AddWithValue("@email", email);

        int temp = Convert.ToInt32(com.ExecuteScalar().ToString());

        conn.Close();
        return temp;
    }
    public static void ChangeUserPasswordDB(string email, string password)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
        conn.Open();

        SqlCommand command = new SqlCommand("update Accounts set password = @pass where email = @email", conn);
        command.Parameters.AddWithValue("@pass", password);
        command.Parameters.AddWithValue("@email", email);
        command.ExecuteNonQuery();

        conn.Close();
    }

    public static void UpdateUserAddress(string TextBox_Email, string TextBox_Address1, string TextBox_Address2, string TextBox_Zipcode)
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString);
        conn.Open();
        using (SqlCommand command = new SqlCommand("update [accounts] set address1=@addr1, address2=@addr2, zipcode=@zipcode WHERE email = @email", conn))
        {
            //checks if the email that the user has entered exists in the database table
            command.Parameters.AddWithValue("email", TextBox_Email);
            command.Parameters.AddWithValue("@addr1", TextBox_Address1);
            command.Parameters.AddWithValue("@addr2", TextBox_Address2);
            command.Parameters.AddWithValue("zipcode", TextBox_Zipcode);
            command.ExecuteNonQuery();
        }
        conn.Close();
    }
}