"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/LobbyHub").build();

//Disable the button until connection is established.
// document.getElementById("findGame").disabled = true;


// registering function that gets called by server (to report total number of clients on server)
connection.on("ClientsOnServer",(value) => {
    var clientsConnected = document.getElementById("clientsConnected");
    clientsConnected.innerText = value.toString();
})

// reporting to server that we have connected
// function ClientConnected() {
//     connection.send("ClientConnected");
// }


connection.start().then(function () {
    // ClientConnected();
    // document.getElementById("findGame").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});


// document.getElementById("findGame").addEventListener("click", function (event) {
//     var user = document.getElementById("userInput").value;
//     var message = document.getElementById("messageInput").value;
//     connection.invoke("SendMessage", user, message).catch(function (err) {
//         return console.error(err.toString());
//     });
//     event.preventDefault();
// });