﻿namespace Trenning_NotificationsExample.Models
{
    public class InactivePassport
    {
        public int ObjectId { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public bool IsActive { get; set; }
    }
}