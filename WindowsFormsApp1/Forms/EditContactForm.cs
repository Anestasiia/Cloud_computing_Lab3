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
using Azure.Storage.Blobs;

namespace WindowsFormsApp1.Forms
{
    public partial class EditContactForm : Form
    {
        private readonly string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=cloudtechnologieslab3;AccountKey=5QmPWRr2jSUr6xhWlv6V1eqks3FRyKOAQF8431EGIBDWZJ1a3hNmTdrzle9u5rl6JaSA2RUHrPV0+AStMcAGUg==;EndpointSuffix=core.windows.net";
        private readonly string TableName = "PhoneBook";
        private readonly string ContainerName = "images";

        private string PathOfFile = String.Empty;

        private string UrlOfImage = String.Empty;

        private string Url;

        //ідентифікатор запису в таблиці Ажур
        private readonly string RowRey;

        public EditContactForm(Contact contact)
        {
            InitializeComponent();

            FirstNameTextBox.Text = contact.FirstName;
            LastNameTextBox.Text = contact.LastName;
            MiddleNameTextBox.Text = contact.MiddleName;
            EmailTextBox.Text = contact.Email;
            PhoneTextBox.Text = contact.Phone;
            AddressTextBox.Text = contact.Address;
            Url = contact.Image;
            if (Url != String.Empty) pictureBox1.Load(Url);
            RowRey = contact.RowKey;
        }
        private TableClient GetTableClient()
        {
            var client = new TableClient(ConnectionString, TableName);
            client.CreateIfNotExists();
            return client;
        }
        private BlobContainerClient GetContainerClient()
        {
            var blobServiceClient = new BlobServiceClient(ConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
            return blobContainerClient;
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (PathOfFile != String.Empty && Url == String.Empty)
            {
                var fileName = Guid.NewGuid().ToString() + ".png";

                var containerClient = GetContainerClient();
                var blobClient = containerClient.GetBlobClient(fileName);
                blobClient.Upload(PathOfFile, true);

                UrlOfImage = $"https://cloudtechnologieslab3.blob.core.windows.net/images/{fileName}";
            }
            else if (PathOfFile != String.Empty)
            {
                var fileName = Url.Split('/').Last();

                var containerClient = GetContainerClient();
                var blobClient = containerClient.GetBlobClient(fileName);
                blobClient.Upload(PathOfFile, true);
            }

            var tableClient = GetTableClient();

            Contact editContact = tableClient.GetEntity<Contact>("Contact", RowRey);
            editContact.FirstName = FirstNameTextBox.Text;
            editContact.LastName = LastNameTextBox.Text;
            editContact.MiddleName = MiddleNameTextBox.Text;
            editContact.Email = EmailTextBox.Text;
            editContact.Phone = PhoneTextBox.Text;
            editContact.Address = AddressTextBox.Text;
            editContact.Image = UrlOfImage != String.Empty ? UrlOfImage : String.Empty;
            tableClient.UpdateEntity(editContact, ETag.All);
            this.Close();
        }

        private void UploadImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = @"jpg files(.*jpg)|*.jpg| PNG files(.*png)|*.png| All Files(*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                PathOfFile = ofd.FileName;
                pictureBox1.ImageLocation = ofd.FileName;
            }
        }
    }
}
