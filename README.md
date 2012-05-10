# WebSocket-Chat-Server

A simple chat server based on the WebSocket protocol. Uses C# 4.0 and external libs [Fleck](http://github.com/statianzo/Fleck) and [FridayThe13th] (http://github.com/jprichardson/FridayThe13th).

See [SteffenSchuette/WebSocket-Chat-Server](http://github.com/FlorianWolters/WebSocket-Chat-Client) for an example WebSocket chat client using HTML5, jQuery and CSS.

## Features

* The chat client automatically tries to establish a connection to the chat server on startup.
* The user can connect and disconnect the client via buttons manually.
* The user can enter a username to use in the chat.
* Chat messages can be send via the return key on the keyboard or a button.
* A chat message (and the output) consists of the timestamp, the username and the text of the message.
* Application Programming Interface (API) documentation with [JSDoc](http://code.google.com/p/jsdoc-toolkit/w)

## Usage

1. [optional] Start a WebSocket chat server which implements the defined chat protocol (see below).
2. [optional] Edit the connection options in the configuration file `assets/js/configuration.js` to match with the WenSocket chat server.
3. Start the `index.html` in a web browser.

# Chat Protocol

All you need to know to implement you own WebSocket chat server is the following:

```js
var jsonObj = {
    // The current UNIX timestamp.
    'ts': Math.round(Date.now() / 1000),
    // The name of the chat user.
    'uid': uid,
    // The text of the chat message.
    'msg': msg
};
```

You expect a JSON object that contains three elements (`ts`, `uid`, `msg`). After the processing of the incoming message you have to send a multicast (or broadcast) to all connected chat clients (including the one that has send the message).

**NOTE:** The message sent, has to be identical to the JSON object described above.

## Used Technologies

* [jQuery](http://jquery.com) v1.7.2
* [HTML5](http://w3.org/TR/html5)
* [CSS](http://w3.org/Style/CSS)
* [normalize.css](http://necolas.github.com/normalize.css)

## TODO

* Refactor JavaScript source code (`assets/js/chatclient.js` and `assets/js/configuration.js`) to use object-oriented programming (OOP).
* Correct JSDoc documentation (currently only the file comments appear within the HTML output of jsdoc-toolkit).

## License

This program is free software: you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this program. If not, see http://gnu.org/licenses/lgpl.txt.
