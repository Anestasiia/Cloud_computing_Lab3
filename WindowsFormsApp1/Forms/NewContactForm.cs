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
using Azure.Storage.Blobs;

namespace WindowsFormsApp1
{
    public partial class NewContactForm : Form
    {
        private readonly string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=cloudtechnologieslab3;AccountKey=5QmPWRr2jSUr6xhWlv6V1eqks3FRyKOAQF8431EGIBDWZJ1a3hNmTdrzle9u5rl6JaSA2RUHrPV0+AStMcAGUg==;EndpointSuffix=core.windows.net";
        private readonly string TableName = "PhoneBook";
        private readonly string ContainerName = "images";
        private string PathOfFile = String.Empty;

        public NewContactForm()
        {
            InitializeComponent();
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

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var _imageUrl = String.Empty;

            if (PathOfFile != String.Empty)
            {
                var fileName = $"{Guid.NewGuid().ToString()}.png";
                var containerClient = GetContainerClient();
                var blobClient = containerClient.GetBlobClient(fileName);
                blobClient.Upload(PathOfFile, true);
                _imageUrl = $"https://cloudtechnologieslab3.blob.core.windows.net/images/{fileName}";
            }

            var newContact = new Contact
            {
                PartitionKey = "Contact",
                RowKey = Guid.NewGuid().ToString(),
                FirstName = FirstNameTextBox.Text,
                LastName = LastNameTextBox.Text,
                MiddleName = MiddleNameTextBox.Text,
                Email = EmailTextBox.Text,
                Address = AddressTextBox.Text,
                Phone = PhoneTextBox.Text,
                Image = _imageUrl,
            };

            var tableClient = GetTableClient();
            tableClient.AddEntity(newContact);

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
