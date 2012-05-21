//-----------------------------------------------------------------------------------
// <copyright file="WebSocketChatServer.cs" company="Hochschule Bremen">
//     Copyright (c) 2012 Steffen Schuette and Florian Wolters. All rights reserved.
// </copyright>
// 
// <author>Steffen Schuette</author><email>steffen.schuette@web.de</email>
// <author>Florian Wolters</author><email>florian.wolters85@googlemail.com</email>
//
// <version>0.1.0-beta</version>
//-----------------------------------------------------------------------------------

namespace WebSocketServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fleck;
    using FridayThe13th;

    /// <summary>
    /// Simple WebSocket Chat Server example using the Fleck C# WebSocket implementation. 
    /// This example also utilises the FridayThe13th library to parse JSON objects.
    /// </summary>
    public class WebSocketChatServer
    {
        /// <summary>
        /// Simple main method creating the Fleck WebSocketServer and listens
        /// </summary>
        /// <param name="args">Command line arguments: hostname port</param>
        private static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Illegal arguments. Usage: <hostname> <port>");
                Console.WriteLine("Press key to close WebSocket server...");

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
            List<IWebSocketConnection> connectedSockets = new List<IWebSocketConnection>();

            // Fleck WebSocketServer object
            Fleck.WebSocketServer server = null;

            // FridayThe13th JSON parser object
            JsonParser jsonParser = null;

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

                        // Add the new connected client to the list handling all connected clients
                        connectedSockets.Add(socket);
                    };

                    // Method called when a client disconnects
                    socket.OnClose = () =>
                    {
                        FleckLog.Info("Client <" + socket.ConnectionInfo.ClientIpAddress + "> disconnected");

                        if (connectedSockets.Contains(socket))
                        {
                            // Remove the new connected client to the list handling all connected clients
                            connectedSockets.Remove(socket);
                        }
                    };

                    // Method called when a client sends a message
                    socket.OnMessage = message =>
                    {
                        // Create FridayThe13th JSON Parser object
                        jsonParser = new JsonParser()
                        {
                            CamelizeProperties = false
                        };

                        try
                        {
                            // Extract fields from the received JSON object
                            dynamic json = jsonParser.Parse(message);

                            // Get the current UNIX timestamp
                            double ts = json.ts;

                            string timestamp = string.Empty;

                            // Get user id from json object
                            string uid = json.uid;

                            // Gut chat message from json object
                            string msg = json.msg;

                            // Calculate timestamp from the received UNIX seconds since 1970-01-01.
                            DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

                            // Add the received timestamp         
                            dateTime = dateTime.AddSeconds(ts);
                            
                            // Build a string representation with the format
                            // <yyyy-mm-dd hh:mm:ss> e.g. <2012-05-10 22:56:26>
                            timestamp = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

                            FleckLog.Info("Msg rcv: " + uid + " @ " + timestamp + " => " + msg);

                            // Update all connected clients
                            connectedSockets.ToList().ForEach(s => s.Send(message));
                        }
                        catch (Exception e)
                        {
                            // Received message exception (e.g. unknown JSON object)
                            FleckLog.Error(e.ToString());
                        }
                    }; // END socket.OnMessage = message =>
                }); // END server.Start(socket =>
            }
            catch (Exception e)
            {
                FleckLog.Error("Error opening WebSocket on <" + connectionInfo + ">");
                FleckLog.Error(e.ToString());

                // Wait until key is pressed
                Console.ReadLine();

                // Close application
                System.Environment.Exit(-1);
            }

            // Read input from the console
            consoleInput = Console.ReadLine();

            while (consoleInput != "exit")
            {
                // loop until user enters "exit" in the console windows
            }
        } // END static void Main(string[] args)
    } // END class WebSocketExample
}