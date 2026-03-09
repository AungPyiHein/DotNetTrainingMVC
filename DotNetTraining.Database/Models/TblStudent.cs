using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class TblStudent
{
    public int StudentId { get; set; }

    public string StudentCode { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public bool DeleteFlag { get; set; }
}
