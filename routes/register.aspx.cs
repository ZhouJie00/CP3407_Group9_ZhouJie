using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Salt_Password_Sample;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Net;
using System.Diagnostics;
using static QRCoder.PayloadGenerator;
using System.Xml.Linq;

namespace AWAD_Assignment.routes
{
    public partial class register : BasePage {
        protected void Page_Load(object sender, EventArgs e) {
        }
        protected void btnRegister_Click(object sender, EventArgs e) {
            RegisterAccount();
        }
 
 
    }
}