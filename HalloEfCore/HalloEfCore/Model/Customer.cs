﻿namespace HalloEfCore.Model
{
    public class Customer : Person
    {
        public string Address { get; set; } = string.Empty;

        public virtual Employee? Employee { get; set; }
    }
}
