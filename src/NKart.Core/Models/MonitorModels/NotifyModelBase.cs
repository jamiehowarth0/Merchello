﻿namespace NKart.Core.Models.MonitorModels
{
    /// <summary>
    /// Defines the base MonitorModel
    /// </summary>
    public abstract class NotifyModelBase : INotifyModel
    {
        /// <summary>
        /// Gets or sets an array of additional recipients (ex. email addresses, phone numbers, twitter handles ...)
        /// </summary>
        public string[] Contacts { get; set; }
    }
}