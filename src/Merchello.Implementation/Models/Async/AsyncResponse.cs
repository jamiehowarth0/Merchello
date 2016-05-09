﻿namespace Merchello.Implementation.Models.Async
{
    using System.Collections.Generic;

    /// <summary>
    /// A class used for returning a responses to an AJAX post to SurfaceControllers.
    /// </summary>
    public class AsyncResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncResponse"/> class.
        /// </summary>
        public AsyncResponse()
        {
            this.ErrorMessages = new List<string>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets the error messages.
        /// </summary>
        public IList<string> ErrorMessages { get; private set; }
    }
}