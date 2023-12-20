#nullable enable
using System;
using Azure;
using Azure.Data.Tables;

namespace WindowsFormsApp1;

public class Contact: ITableEntity
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }

    public string Phone { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string MiddleName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Image { get; set; }

    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}