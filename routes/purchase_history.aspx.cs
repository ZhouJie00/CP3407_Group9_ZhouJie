﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AWAD_Assignment.routes
{
    public partial class purchase_history : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RepeaterHistory.DataSource = Function.GetOrderHistory(Account.GetAccount(Session["email"].ToString()).id);
            RepeaterHistory.DataBind();

        }


        protected void RepeaterHistoryChild_DataBinding(object sender, EventArgs e)
        {
            Repeater rep = (Repeater)(sender);

            string order_id = (string)(Eval("Id"));

            // Assuming you have a function call `GetSomeData` that will return
            // the data you want to bind to your child repeater.
            rep.DataSource = Function.GetPurchaseHistory(order_id);
            rep.DataBind();
        }
    }
}