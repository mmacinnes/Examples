"use strict";
// Base script came from "Tutorial: Get started with ASP.NET CoreSignalR"

let connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build()

//Disable the buttons until connection is established.
document.getElementById("sendButton").disabled = true;
document.getElementById("userInput").focus();
//=======================================================================================
// Start - setup callbacks for Client Methods 
//
connection.on("ReceiveMessage", function (user, message) {
  var li = document.createElement("li");
  document.getElementById("messagesList").appendChild(li);
  // We can assign user-supplied strings to an element's textContent because it
  // is not interpreted as markup. If you're assigning in any other way, you
  // should be aware of possible script injection concerns.
  li.textContent = `${user} says ${message}`;
});

connection.on("ReceiveList", function (message) {
  var ul = document.getElementById("usersList");
  // possible script injection concern
  ul.innerHTML = message;
});
//
// End  - Client Method calls setup callbacks
//============================================================================================

// Start the connection, if successful list users who have already sent messages
connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
  })
  .then(function () {connection.invoke("ListUsers")})
  .catch(function (err) {
    return console.error(err.toString());
  });


//====================================================================================
//  Start - Setup Listeners
//

// Send Button
document.getElementById("sendButton").addEventListener("click",
 function (event) {
    var sendTo = document.getElementById("sendToInput").value; 
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message, sendTo).catch(function (err) {
      return console.error(err.toString());
    });
    event.preventDefault();
  }
);

// remove user from list if the page is closed
window.addEventListener("beforeunload", function (event) { 
  var user = document.getElementById("userInput").value;
  connection.invoke("ClosePage", user).catch(function (err) {
    return console.error(err.toString())
  })
});
//
//  End   - Setup Listeners
//=====================================================================================
