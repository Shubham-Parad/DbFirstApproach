﻿using System;
using System.Collections.Generic;

namespace DBFirstApproach.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }

    public string? Password { get; set; }
}
