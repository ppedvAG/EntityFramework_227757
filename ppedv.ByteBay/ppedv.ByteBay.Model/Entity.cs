﻿namespace ppedv.ByteBay.Model
{
    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Modified { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
    }
}