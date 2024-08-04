using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace AWAD_Assignment.routes
{
    public partial class search : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["q"]))
                {
                    string searchInput = Server.HtmlDecode(Request.QueryString["q"]);
                    PerformSearch(searchInput);
                }
                else
                {
                    // Redirect to home if no query provided
                    Response.Redirect("home");
                }
            }
        }
    }
}