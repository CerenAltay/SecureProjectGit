﻿using SecureWebsitePractices.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SecureWebsitePractices
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var searchTerm = Request.QueryString["q"];
            SearchTerm.Text = searchTerm;

            var result = new List<ProductModel>();

            ProductModel Products = new ProductModel();
         
            Products.Searched = Request.QueryString["q"];


            var sqlString = "SELECT * FROM Product";
            var connString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var conn = new SqlConnection(connString))
            {
                var command = new SqlCommand(sqlString, conn);
                command.Connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Products.ProductKey = reader.GetInt32(0).ToString();
                        Products.ProductAlternateKey = reader.GetString(1);
                        Products.ProductName = reader.GetString(5);
                        Products.StockLevel = reader.GetInt16(9);
                    }
                        result.Add(Products);
                }
            }
            SearchGrid.DataSource = result.Where(p => p.ProductName.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0);
            SearchGrid.DataBind();
        }
    }
}