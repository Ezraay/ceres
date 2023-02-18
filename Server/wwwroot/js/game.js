"use strict";

var GameHubConnection = new signalR.HubConnectionBuilder()
    // .withHubProtocol(new signalR.JsonHubProtocol({
    //     transferFormat: signalR.TransferFormat.Text,
    //     typeNameHandling: 2  // Equivalent to TypeNameHandling.All
    // }))
    .withUrl("/GameHub").build();
var Player1Turn = true;

let userId = sessionStorage.getItem("userId");
let gameId = sessionStorage.getItem("gameId");


function fulfilled() {
    if (gameId == null){                            // joining as a spectator - getting gameid from the url
        const queryString = window.location.search;
        const urlParams = new URLSearchParams(queryString);
        gameId =  urlParams.get('gameid');
    }
    
    
    GameHubConnection.invoke("JoinGame", gameId, userId).then((res) => {
        console.log(res)
        if (res == "JoinedAsPlayer1") {
            document.getElementById("P2Action").hidden = false;
            document.getElementById("P1Action").disabled =false;
            // document.getElementById("P1Action").disabled = !Player1Turn;
        }
        if (res == "JoinedAsPlayer2") {
            document.getElementById("P1Action").hidden = false;
            document.getElementById("P2Action").disabled =false;
        }
        
    });
    
}

function rejected() {

}


GameHubConnection.on("UpdatePlayersName", (p1Name, p2Name) => {
    console.log("UpdatePlayersName")
    document.getElementById("player1Name").innerText = p1Name;
    document.getElementById("player2Name").innerText = p2Name;
    // document.getElementById("P1Action").disabled = false;
})

GameHubConnection.on("ServerAction", action => {
    console.log("ServerAction");
    console.log(action);
});


GameHubConnection.start().then(fulfilled, rejected)
.catch(function (err) {
    return console.error(err.toString());
});




document.getElementById("P1Action").addEventListener("click", PlayerCommand);
document.getElementById("P2Action").addEventListener("click", PlayerCommand);

function PlayerCommand(){
    console.log("sending command");
    var playerCommand = new Object();
    GameHubConnection.send("PlayerSentCommand", gameId, userId, playerCommand).catch(function (err) {
        return console.error(err.toString());
    });
    // document.getElementById("P1Action").disabled = !Player1Turn;
    // document.getElementById("P2Action").disabled = Player1Turn;

}