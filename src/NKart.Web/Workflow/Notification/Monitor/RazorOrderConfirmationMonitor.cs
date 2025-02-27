﻿namespace NKart.Web.Workflow.Notification.Monitor
{
    using System.Linq;

    using NKart.Core.Gateways.Notification;
    using NKart.Core.Gateways.Notification.Monitors;
    using NKart.Core.Gateways.Notification.Triggering;
    using NKart.Core.Models.MonitorModels;
    using NKart.Core.Observation;

    /// <summary>
    /// A razor based order confirmation monitor.
    /// </summary>
    [MonitorFor("8DA9C3DA-1169-4E36-9D35-C8DF2C52CD93", typeof(OrderConfirmationTrigger), "Order Confirmation (Razor)", true)]
    public class RazorOrderConfirmationMonitor : RazorMonitorBase<IPaymentResultMonitorModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RazorOrderConfirmationMonitor"/> class.
        /// </summary>
        /// <param name="notificationContext">
        /// The notification context.
        /// </param>
        public RazorOrderConfirmationMonitor(INotificationContext notificationContext)
            : base(notificationContext)
        {
        }

        /// <summary>
        /// The on next.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public override void OnNext(IPaymentResultMonitorModel value)
        {
            if (!value.PaymentSuccess) return;

            base.OnNext(value);
        }
    }
}