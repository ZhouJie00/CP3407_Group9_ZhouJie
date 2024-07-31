﻿using System;
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
public class Clothes {
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
    public Clothes(string id, string name, int quantity, decimal price, string overview, char gender, int category_id, string link, DateTime dateAdded) {
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
                } else
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