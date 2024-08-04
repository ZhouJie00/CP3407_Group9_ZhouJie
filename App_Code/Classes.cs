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

/// <summary>
/// Summary description for Clothes
/// </summary>
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

    // GET / SET
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

    /// <summary>
    /// Shopping Cart Object
    /// </summary>
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

/// <summary>
/// Account class for accounts table
/// </summary>
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

    // GET / SET
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

    // Methods
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
        else
        {
            account = null;
        }
        conn.Close();
        dr.Close();
        dr.Dispose();
        return account;
    }

    public static GridView GetAllUsers(GridView gridView)
    {
        DataTable dt = new DataTable();
        using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Database"].ConnectionString))
        {
            using (SqlCommand command = new SqlCommand("select * from Accounts", connection))
            {
                connection.Open();

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
                    gridView.Rows[0].Cells[0].Text = "No Users in Database";
                    gridView.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    gridView.Rows[0].Cells[0].VerticalAlign = VerticalAlign.Middle;
                }
            }
        }
        return gridView;
    }

 
}