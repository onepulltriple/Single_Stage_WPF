﻿using System;
using System.Collections.Generic;

namespace SINGLE_STAGE.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
