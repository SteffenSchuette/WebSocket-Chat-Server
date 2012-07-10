//-----------------------------------------------------------------------------------
// <copyright file="ChatMessage.cs" company="Hochschule Bremen">
//     Copyright (c) 2012 Steffen Schuette. All rights reserved.
// </copyright>
// <author>Steffen Schuette</author><email>steffen.schuette@web.de</email>
// <version>0.1.0-beta</version>
//-----------------------------------------------------------------------------------

namespace WebSocketServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Chat message used to send chat messages to the connected clients.
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// Gets or sets the Timestamp string with the format "YYYY-MM-DD HH-MM-SS" like "2012-07-05 19:04:00".
        /// </summary>
        public string TS { get; set; }

        /// <summary>
        /// Gets or sets the user name used by the client.
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// Gets or sets the chat message.
        /// </summary>
        public string MSG { get; set; }
    }
}
