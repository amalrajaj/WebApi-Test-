using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class StudentDbTable
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } 

    public DateTime Dob { get; set; }

    public string Email { get; set; } 

    public int Class { get; set; }

    public virtual ICollection<Stunthobby> Stunthobbies { get; set; } = new List<Stunthobby>();
}
