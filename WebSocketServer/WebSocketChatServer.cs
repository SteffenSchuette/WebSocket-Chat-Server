//-----------------------------------------------------------------------------------
// <copyright file="WebSocketChatServer.cs" company="Hochschule Bremen">
//     Copyright (c) 2012 Steffen Schuette. All rights reserved.
// </copyright>
// <author>Steffen Schuette</author><email>steffen.schuette@web.de</email>
// <version>0.2.0-beta</version>
//-----------------------------------------------------------------------------------

namespace WebSocketServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fleck;
    using Newtonsoft.Json;

    /// <summary>
    /// Simple WebSocket Chat Server example using the Fleck C# WebSocket implementation. 
    /// This example also utilises the Newtonsoft.Json library to build JSON objects.
    /// </summary>
    public class WebSocketChatServer
    {
        /// <summary>
        /// Simple main method creating the Fleck WebSocketServer
        /// </summary>
        /// <param name="args">Command line arguments: hostname port</param>
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Illegal arguments. Usage: <hostname> <port>");
                Console.WriteLine("Press <RETURN> to close WebSocket server...");

                // Wait until key is pressed
                Console.ReadLine();

                // Close application
                System.Environment.Exit(-1);
            }

            // <hostname> to bind the server socket to
            string hostname = string.Empty;

            // <port> to bind the server socket to
            int port = 0;

            // Set the fleck logging level
            FleckLog.Level = LogLevel.Info;

            // List containing all connected clients
            Dictionary<Fleck.IWebSocketConnection, string> connectedSockets = new Dictionary<IWebSocketConnection, string>();

            // Fleck WebSocketServer object
            Fleck.WebSocketServer server = null;

            // Sets the connection information how to open the WebSocket
            string connectionInfo = string.Empty;

            // Variable for the server console input
            string consoleInput = string.Empty;

            try
            {
                hostname = args[0];

                port = int.Parse(args[1]);

                connectionInfo = "ws://" + hostname + ":" + port;

                // Instatiate WebSocketServer object
                server = new Fleck.WebSocketServer(connectionInfo);

                // Open the WebSocket on the given port
                server.Start(socket =>
                {
                    // Method called when a client connects
                    socket.OnOpen = () =>
                    {
                        FleckLog.Info("Client <" + socket.ConnectionInfo.ClientIpAddress + "> connected");
                    };

                    // Method called when a client disconnects
                    socket.OnClose = () =>
                    {
                        // If socket is stored in connected sockets list, remove it
                        if (connectedSockets.ContainsKey(socket))
                        {
                            connectedSockets.Remove(socket);
                        }
                    };

                    // Method called when a client sends a message
                    socket.OnMessage = message =>
                    {
                        // Timestamp containing the current time with the pattern: 2012-07-06 19:04:23
                        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        
                        // Get user id from receivedJSON object
                        string uid = string.Empty;

                        // The chat message object with the TS (timestamp), UID (user id) and MSG (message) properties.
                        ChatMessage chatMessage = new ChatMessage(); 

                        try
                        {
                            if (connectedSockets.ContainsKey(socket))
                            {
                                // Get the client user name
                                uid = connectedSockets[socket];

                                // Build the JSON object which will be send to all connected clients.
                                // It contains the current timestamp, the user name the message is 
                                // came from and finally the message itself.
                                chatMessage.UID = uid;
                                chatMessage.MSG = message;

                                // Socket already stored in connected sockets and send its user name. So send
                                // assembled JSON object to all connected clients.
                                foreach (KeyValuePair<Fleck.IWebSocketConnection, string> client in connectedSockets)
                                {
                                    client.Key.Send(JsonConvert.SerializeObject(chatMessage));
                                    FleckLog.Info("Msg rcv: " + uid + " @ " + timestamp + " => " + message);
                                }
                            } // END if (connectedSockets.ContainsKey(socket))
                            else
                            {
                                // First message from the socket => message contains the client user name.
                                // Check now if user name is available. If not, add socket to the connected 
                                // sockets containing its user name.
                                if (!connectedSockets.ContainsValue(message))
                                {
                                    // Store new connected client with its send user name to the connected sockets.
                                    connectedSockets.Add(socket, message);
                                    FleckLog.Info("Client <" + socket.ConnectionInfo.ClientIpAddress + "> set user name to <" + message + ">");
                                }
                                else
                                {
                                    // Send client that the user name is already in use. The server now has
                                    // to close the WebSocket to the client
                                    chatMessage.TS = timestamp;
                                    chatMessage.UID = uid;
                                    chatMessage.MSG = "Error: the user name <" + message + "> is already in use!";

                                    // Serialise ChatMessage object to JSON
                                    socket.Send(JsonConvert.SerializeObject(chatMessage));
                                    socket.Close();

                                    // If socket is stored in connected sockets list, remove it
                                    if (connectedSockets.ContainsKey(socket))
                                    {
                                        connectedSockets.Remove(socket);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            FleckLog.Error(e.ToString());
                        }
                    }; // END socket.OnMessage = message =>
                }); // END server.Start(socket =>
            }
            catch (Exception e)
            {
                // WebSocket could not be bind. E.g. if the socket is already in use.
                FleckLog.Error("Error opening WebSocket on <" + connectionInfo + ">. WebSocket maybe in use?");
                FleckLog.Error("Exception string: \n" + e.ToString());

                // Wait until key is pressed
                Console.ReadLine();

                // Close application
                System.Environment.Exit(-1);
            }

            // Loop until the user enters "exit" in the console window to close the application
            while (Console.ReadLine() != "exit")
            {
                // loop until user enters "exit" in the console windows
            }
        } // END static void Main(string[] args)
    } // END class WebSocketExample
}