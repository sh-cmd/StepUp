using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

public partial class SignIn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if( ! IsPostBack )
        {
            if( Request.Cookies["UNAME"] != null && Request.Cookies["UPWD"] != null )
            {
                txtUsername_In.Text = Request.Cookies["UNAME"].Value;
                txtPass_In.Text = Request.Cookies["UPWD"].Value;
                CheckBox1.Checked = true;
            }
        }
    }

    protected void btnlogin_Click(object sender, EventArgs e)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["StepUp"].ConnectionString))
        {
            //Response.Write("<script> alert('Hey!'); </script>");
            con.Open();
            SqlCommand cmd = new SqlCommand("Select * from tblUsers where Username=@username and Password=@pwd", con);
            cmd.Parameters.AddWithValue("@username", txtUsername_In.Text.Trim()) ;
            cmd.Parameters.AddWithValue("@pwd", txtPass_In.Text.Trim());
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            //Response.Write("<script> alert('"+dt.Rows.Count+"'); </script>");
            //Response.Write("<script> alert('" + txtUsername.Text + "'); </script>");
            //Response.Write("<script> alert('" + txtPass.Text + "'); </script>");
            //Response.Write("<script> alert('username' , ' "+username+" '); </script>");
            if (dt.Rows.Count != 0)
            {
                //Response.Write("<script> alert('Hey!!!'); </script>");
                if (CheckBox1.Checked)
                {
                    Response.Cookies["UNAME"].Value = txtUsername_In.Text.Trim();
                    Response.Cookies["UPWD"].Value = txtPass_In.Text.Trim();

                    Response.Cookies["UNAME"].Expires = DateTime.Now.AddDays(10);
                    Response.Cookies["UPWD"].Expires = DateTime.Now.AddDays(10);

                }
                else
                {
                    Response.Cookies["UNAME"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["UPWD"].Expires = DateTime.Now.AddDays(-1);
                }
                
                string Utype;
                Utype = dt.Rows[0][5].ToString().Trim();

                if(Utype=="User")
                {
                    Session["Username"] = txtUsername_In.Text.Trim();
                    Response.Redirect("~/UserHome.aspx");
                }
                else if(Utype=="Admin")
                {
                    Session["Username"] = txtUsername_In.Text.Trim();
                    Response.Redirect("~/AdminHomePage1.aspx");
                }

            }
            else
            {
                Response.Write("<script> alert('Please try again :('); </script>");
            }
            Response.Write("<script> alert('Welcome! Login successfull. Happy Shopping:)'); </script>");
            clr();
            con.Close();

        }
    }

    private void clr()
    {
        txtPass_In.Text = string.Empty;
        txtUsername_In.Text = string.Empty;
        txtUserName.Focus();
    }

    private bool isformvalid()
    {

        if (txtUname_Up.Text == "")
        {
            Response.Write("<script> alert('Please enter valid username'); </script>");
            txtUname_Up.Focus();
            return false;
        }
        else if (txtPass_Up.Text == "")
        {
            Response.Write("<script> alert('Please enter password'); </script>");
            txtPass.Focus();
            return false;
        }
        else if (!txtPass_Up.Text.Equals(txtCPass_Up.Text))
        {
            Response.Write("<script> alert('Password and confirm password don't match.'); </script>");
            txtCPass_Up.Focus();
            txtPass.Focus();
            return false;
        }
        else if (txtName_Up.Text == "")
        {
            Response.Write("<script> alert('Please enter your name'); </script>");
            txtName_Up.Focus();
            return false;
        }
        else if (txtEmail_Up.Text == "")
        {
            Response.Write("<script> alert('Please enter Email ID'); </script>");
            txtEmail_Up.Focus();
            return false;
        }

        return true;

    }

    private void clear()
    {
        txtEmail_Up.Text = string.Empty;
        txtUname_Up.Text = string.Empty;
        txtPass_Up.Text = string.Empty;
        txtCPass_Up.Text = string.Empty;
        txtName_Up.Text = string.Empty;
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {

        {
            if (isformvalid())
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["StepUp"].ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("Insert into tblUsers(Username,Password,Email,Name,Usertype) Values('" + txtUname_Up.Text.Trim() + "', '" + txtPass_Up.Text.Trim() + "', '" + txtEmail_Up.Text.Trim() + "', '" + txtName_Up.Text.Trim() + "','User')", con);
                    cmd.ExecuteNonQuery();
                    Response.Write("<script> alert('Welcome! Registeration successfull. Happy Shopping:)'); </script>");
                    clear();
                    con.Close();

                }
                Response.Redirect("~/SignIn.aspx");
            }
            else
            {
                Response.Write("<script> alert('Registeration Failed :( Try again!'); </script>");
            }

        }
    }
}