using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Stunthobby
{
    public int Id { get; set; }

    public int HobbyId { get; set; }

    public int StudentId { get; set; }

    public virtual HobbyTable Hobby { get; set; } = null!;

    public virtual StudentDbTable Student { get; set; } = null!;
}
