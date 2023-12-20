using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Azure;
using System.Runtime.InteropServices.ComTypes;
using WindowsFormsApp1.Forms;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace WindowsFormsApp1
{
    public partial class ContactForm : Form
    {
        private readonly string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=cloudtechnologieslab3;AccountKey=5QmPWRr2jSUr6xhWlv6V1eqks3FRyKOAQF8431EGIBDWZJ1a3hNmTdrzle9u5rl6JaSA2RUHrPV0+AStMcAGUg==;EndpointSuffix=core.windows.net";
        private readonly string TableName = "PhoneBook";
        private readonly string ContainerName = "images";

        public ContactForm()
        {
            InitializeComponent();

            PopulateTableData();

            CreateEditAndDeleteButtons();

            ChangeColumnOrder();
        }

        private TableClient GetTableClient()
        {
            var client = new TableClient(ConnectionString, TableName);
            client.CreateIfNotExists();
            return client;
        }
        private IEnumerable<Contact> GetContacts()
        {
            var client = GetTableClient();

            Pageable<TableEntity> oDataQueryEntities = client.Query<TableEntity>();

            var result = new List<Contact>();

            foreach (var entity in oDataQueryEntities)
            {
                var newContact = new Contact
                {
                    PartitionKey = entity.PartitionKey,
                    RowKey = entity.RowKey,
                    Timestamp = entity.Timestamp,
                    ETag = entity.ETag,
                    FirstName = entity.GetString("FirstName"),
                    LastName = entity.GetString("LastName"),
                    MiddleName = entity.GetString("MiddleName"),
                    Email = entity.GetString("Email"),
                    Address = entity.GetString("Address"),
                    Phone = entity.GetString("Phone"),
                    Image = entity.GetString("Image"),
                };

                result.Add(newContact);
            }

            return result;
        }

        private void PopulateTableData()
        {
            List<Contact> contacts = GetContacts().ToList();

            TableData.DataSource = contacts.Select(c => new { c.RowKey, c.FirstName, c.LastName, c.MiddleName, c.Email, c.Address, c.Phone, c.Image }).ToList();
        }
        private void ChangeColumnOrder()
        {
            List<string> desiredOrder = new List<string>() { "RowKey", "FirstName", "LastName", "MiddleName", "Email", "Address", "Phone", "Image", "Edit", "Delete" };

            foreach (var columnName in desiredOrder)
            {
                DataGridViewColumn column = TableData.Columns[columnName];

                if (column != null)
                {
                    column.DisplayIndex = Array.IndexOf(desiredOrder.ToArray(), columnName);
                }
            }
        }

        private void CreateEditAndDeleteButtons()
        {
            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            editButtonColumn.HeaderText = @"Edit";
            editButtonColumn.Text = "Edit";
            editButtonColumn.UseColumnTextForButtonValue = true;
            TableData.Columns.Add(editButtonColumn);

            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
            deleteButtonColumn.HeaderText = @"Delete";
            deleteButtonColumn.Text = "Delete";
            deleteButtonColumn.UseColumnTextForButtonValue = true;
            TableData.Columns.Add(deleteButtonColumn);
        }

        private void CreateNewContactButton_Click(object sender, EventArgs e)
        {
            NewContactForm createForm = new NewContactForm();

            createForm.Show();
        }

        private void ContactForm_Activated(object sender, EventArgs e)
        {
            PopulateTableData();
        }

        private void TableData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (TableData.Columns[e.ColumnIndex] is DataGridViewButtonColumn))
            {
                int rowIndex = e.RowIndex;

                if (TableData.Columns[e.ColumnIndex].HeaderText == "Delete")
                {
                    var confirmResult = MessageBox.Show(@"Are you sure to delete this item ?",
                        @"Confirm Delete?",
                        MessageBoxButtons.YesNo);

                    if (confirmResult == DialogResult.Yes)
                    {
                        var client = GetTableClient();
                        client.DeleteEntity("Contact", TableData.Rows[rowIndex].Cells[2].Value.ToString());
                        var blobServiceClient = new BlobServiceClient(ConnectionString);
                        var blobContainerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
                        var blobContainer = blobContainerClient.GetBlobClient(TableData.Rows[rowIndex].Cells[9]
                            .Value.ToString().Split('/').Last());
                        blobContainer.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);

                        MessageBox.Show(@"Contact was deleted",
                            @"Success",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                else if (TableData.Columns[e.ColumnIndex].HeaderText == "Edit")
                {
                    var editContact = new Contact
                    {
                        PartitionKey = "Contact",
                        RowKey = TableData.Rows[rowIndex].Cells[2].Value.ToString(),
                        FirstName = TableData.Rows[rowIndex].Cells[3].Value.ToString(),
                        LastName = TableData.Rows[rowIndex].Cells[4].Value.ToString(),
                        MiddleName = TableData.Rows[rowIndex].Cells[5].Value.ToString(),
                        Email = TableData.Rows[rowIndex].Cells[6].Value.ToString(),
                        Address = TableData.Rows[rowIndex].Cells[7].Value.ToString(),
                        Phone = TableData.Rows[rowIndex].Cells[8].Value.ToString(),
                        Image = TableData.Rows[rowIndex].Cells[9].Value.ToString(),
                    };

                    EditContactForm editForm = new EditContactForm(editContact);

                    editForm.Show();
                }
            }
        }
    }
}
